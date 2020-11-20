using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Microsoft.ML.DataOperationsCatalog;

namespace MLFSQLIAD
{
    public partial class Test : System.Web.UI.Page
    {
        // Declare Global Variables
        //static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "burp_suite2.txt");
        static readonly string _dataPath = "D:/UNIVERSITY/CyberSecurity MSc/PROM02/Application/MLFSQLIAD/MLFSQLIAD/Data/test.txt";

        // Create ML.NET context/local environment - allows you to add steps in order to keep everything together
        private MLContext _mlContext;
        private ITransformer _model;
        private EstimatorChain<TransformerChain<KeyToValueMappingTransformer>> _trainingPipeline;
        private static EstimatorChain<KeyToValueMappingTransformer> _trainer;
        private IDataView _data;

        private static string 
            sCatalog = "",
            sTrainer = "",
            sDatabase = "";

        private static int iLabel = -1, iTestNumber = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            _mlContext = null;
            _model = null;
            _trainingPipeline = null;
            _trainer = null;
            _data = null;

            if (!IsPostBack)
            {
                //btnTest.Visible = false;
                ddlCatalog.SelectedIndex = 0;
            }

            PopulateTable();
            GetLastTestNumber();
        }

        private void GetLastTestNumber()
        {
            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();

            string sLastNumber = da.SP_RetrieveDataOneValue("SP_READ_LAST_TEST_NUMBER", sParams);
            
            if (sLastNumber != "")
            {
                lblLastNumber.Text = "Last Num: " + sLastNumber;
                txtTestNumber.Text = (Convert.ToInt32(sLastNumber) + 1).ToString();
            }
            
        }

        private void PopulateTable()
        {
            // Clear panels first
            pnlTest.Controls.Clear(); ;

            // array for column names
            List<string> lColumnNames = new List<string>();
            lColumnNames.Clear();
            lColumnNames.Add("ID");
            lColumnNames.Add("Text");
            lColumnNames.Add("Label");
            lColumnNames.Add("Tools");

            // database column names
            List<string> lDBFieldNames = new List<string>();
            lDBFieldNames.Clear();
            lDBFieldNames.Add("Id");
            lDBFieldNames.Add("Text");
            lDBFieldNames.Add("Label");

            List<string> sParams = new List<string>();
            sParams.Clear();
            //sParams.Add("@Form:" + sFormName);

            HtmlTables ht = new HtmlTables();
            bool bHasrow = ht.SP_populateHtmlTables(pnlTest, "test_table", "SP_READ_SQL_COMMANDS_SETUP", lColumnNames, lDBFieldNames, sParams);

        }

        /*
        private void BuildMLModel()
        {
            // Call Load Data
            splitDataView = LoadData(mlContext);

            // Call Build And Train Model
            model = BuildAndTrainModel(mlContext, splitDataView.TrainSet, txtResult);


            // Call Evaluate
            Evaluate(mlContext, model, splitDataView.TestSet, txtResult);
        }*/

        private void SelectDatabase()
        {
            if (sDatabase == "burp_suite")
            {
                ExportTextFile("SP_READ_BURP");
            }
            else if (sDatabase == "fuzzydb")
            {
                ExportTextFile("SP_READ_FUZZY");
            }
            else if (sDatabase == "OWASP")
            {
                ExportTextFile("SP_READ_OWASP");
            }

            if (File.Exists(_dataPath))
            {
                BuildModel();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Trainer file missing !');", true);
            }
        }

        protected void ExportTextFile(String sStoredProcedure)
        {
            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();

            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProcedure, sParams);

            dt.Load(objDataReader);

            if (dt.Rows.Count > 0)
            {

                //Build the Text file data.
                string txt = string.Empty;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_dataPath))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        txt = "";

                        foreach (DataColumn column in dt.Columns)
                        {
                            //Add the Data rows.
                            txt += row[column.ColumnName].ToString() + "\t";
                        }

                        // Remove last tab
                        txt = txt.Remove(txt.Length - 1, 1);

                        file.WriteLine(txt);
                    }
                }
            }
        }

        private void BuildModel()
        {
            // Set up the MLContext, which is a catalog of components in ML.NET.
            _mlContext = new MLContext();
            // Specify the schema for trainer data and read it into DataView.
            _data = _mlContext.Data.LoadFromTextFile<AppInput>(path: _dataPath, hasHeader: true, separatorChar: '\t');
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                      .Append(_mlContext.Transforms.Text.FeaturizeText("FeaturesText", new Microsoft.ML.Transforms.Text.TextFeaturizingEstimator.Options
                                      {
                                          WordFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options { NgramLength = 2, UseAllLengths = true },
                                          CharFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options { NgramLength = 3, UseAllLengths = false },
                                      }, "Text"))
                                      .Append(_mlContext.Transforms.CopyColumns("Features", "FeaturesText"))
                                      .Append(_mlContext.Transforms.NormalizeLpNorm("Features", "Features"))
                                      .AppendCacheCheckpoint(_mlContext);

            // Set the training algorithm 

            if (sTrainer == "AveragedPerceptron")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "LbfgsLogisticRegression")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "LdSvm")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.LdSvm(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "LinearSvm")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.LinearSvm(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "SdcaLogisticRegression")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "SdcaNonCalibrated")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "SgdCalibrated")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.SgdCalibrated(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }
            else if (sTrainer == "SgdNonCalibrated")
            {
                _trainer = _mlContext.MulticlassClassification.Trainers.OneVersusAll(_mlContext.BinaryClassification.Trainers.SgdNonCalibrated(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label")
                                                  .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            }


            _trainingPipeline = dataProcessPipeline.Append(_trainer);
            

            Train();
        }

        private void Train()
        {
            txtResult.Text += "=============== Create and Train the Model ===============" + Environment.NewLine;
            txtResult.Text += "Catalog: " + sCatalog + Environment.NewLine;
            txtResult.Text += "Trainer: " + sTrainer + Environment.NewLine;
            txtResult.Text += "Database: " + sDatabase + Environment.NewLine;

            //Train model
            _model = _trainingPipeline.Fit(_data);

            txtResult.Text += "==================== End of training =====================" + Environment.NewLine;

            Evaluate();
        }

        private void Evaluate()
        {
            txtResult.Text += "========================= Evaluate ======================" + Environment.NewLine;

            var testDataView = _mlContext.Data.LoadFromTextFile<AppInput>(path: _dataPath, hasHeader: true, separatorChar: '\t');
            var modelMetrics = _mlContext.MulticlassClassification.Evaluate(_model.Transform(testDataView));

            txtResult.Text +=  $"MicroAccuracy: {modelMetrics.MicroAccuracy: 0.###}" + Environment.NewLine;
            txtResult.Text +=  $"MacroAccuracy: {modelMetrics.MacroAccuracy: 0.###}" + Environment.NewLine;
            txtResult.Text +=  $"LogLoss: {modelMetrics.LogLoss: #.###}" + Environment.NewLine;
            txtResult.Text += $"LogLossReduction: {modelMetrics.LogLossReduction: #.###}" + Environment.NewLine;

            txtResult.Text += "==================== End of evaluate =====================" + Environment.NewLine;
        }

        private string UseModelWithSingleItem(TextBox txtResult, String sTestSql)
        {
            // Create Predict Engine
            //PredictionEngine<SqlData, SqlPrediction> predictionFunction = mlContext.Model.CreatePredictionEngine<SqlData, SqlPrediction>(model);

            var predictor = _mlContext.Model.CreatePredictionEngine<AppInput, AppPrediction>(_model);


            // Create Issue
            AppInput issueSql = new AppInput
            {
                Text = sTestSql
            };

            // Predict
            var resultPrediction = predictor.Predict(issueSql);

            // Too many record have to save in short time so we must create a long sql strint instead use ony-by-one insert to history database
            string sSQL = sTestSql.Replace("'", "''");

            string str = "INSERT INTO [dbo].[history] ([TestNumber], [CatalogName], [TrainerName], [DatabaseName], [SqlCommand], [Label], [Prediction], [Probability], [Recorded])"
                + " VALUES (" + iTestNumber + ", N'" + sCatalog + "', N'" + sTrainer + "', N'" + sDatabase + "', N'" + sSQL + "', " + iLabel.ToString() + ", " + resultPrediction.Label
                + ", 0, '" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "');";

            // Output Prediction
            txtResult.Text += Environment.NewLine + "=============== Prediction Test of model with a single sample and test dataset ===============" + Environment.NewLine;
        
            txtResult.Text += $"SQL Command: {sTestSql}" + Environment.NewLine +
                $"Prediction: {((resultPrediction.Label == "1") ? "Positive" : "Negative")}" + Environment.NewLine;

            txtResult.Text += "=============== End of Predictions ===============" + Environment.NewLine;            

            return str;
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@ID:" + hID.Value);

            int ID = da.InsertRecord("SP_DELETE_SQL_COMMAND", sParams);

            PopulateTable();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (txtEditSQL.Text != "")
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();
                sParams.Add("@Text:" + txtEditSQL.Text);
                sParams.Add("@Label:" + Convert.ToInt32(ddlEditLabel.SelectedValue));
                sParams.Add("@ID:" + hID.Value);

                da.SP_CreateUpdateRecord("SP_UPDATE_SQL_COMMAND", sParams);
            }

            PopulateTable();
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            if ( ddlTrainer.SelectedIndex > 0 && ddlDatabase.SelectedIndex > 0)
            {
                iTestNumber = Convert.ToInt32(txtTestNumber.Text);

                sCatalog = ddlCatalog.SelectedValue;
                sTrainer = ddlTrainer.SelectedValue;
                sDatabase = ddlDatabase.SelectedValue;

                SelectDatabase();

                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();

                DataTable dt = new DataTable();
                SqlDataReader objDataReader = da.ExecuteReader("SP_READ_SQL_COMMANDS", sParams);

                dt.Load(objDataReader);

                StringBuilder sb = new StringBuilder();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        iLabel = Convert.ToInt32(dt.Rows[i]["Label"]);

                        sb.Append(UseModelWithSingleItem(txtResult, dt.Rows[i]["Text"].ToString()));

                        Debug.Print("Row: " + i.ToString() + ", SQL: " + dt.Rows[i]["Text"].ToString());
                    }
                }

                // Insert all record to history database
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MLConn"].ConnectionString))
                {
                    string sql = sb.ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('The test completed successfully !');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Please select training data source !');", true);
            }

            
        }

        /*
        protected void rdoDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Print(rdoDataSource.SelectedValue);

            txtResult.Text = "";
            SelectDatabase();
        }*/

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtInsertSQL.Text != "")
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();
                sParams.Add("@Text:" + txtInsertSQL.Text);
                sParams.Add("@Label:" + Convert.ToInt32(ddlInsertLabel.SelectedValue));

                int ID = da.InsertRecord("SP_INSERT_SQL_COMMAND", sParams);
            }

            PopulateTable();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Debug.Print("Add SQL");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "OpenModal();", true);
        }
    }    
}