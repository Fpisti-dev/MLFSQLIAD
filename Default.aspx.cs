using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Microsoft.ML.DataOperationsCatalog;

namespace MLFSQLIAD
{
    public partial class _Default : Page
    {
        // Declare Global Variables
        //static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "burp_suite2.txt");
        static readonly string _dataPath = "D:/UNIVERSITY/CyberSecurity MSc/PROM02/Application/MLFSQLIAD/MLFSQLIAD/Data/data.txt";

        // Create ML.NET context/local environment - allows you to add steps in order to keep everything together
        private static MLContext _mlContext;
        private static ITransformer _model;
        private static EstimatorChain<TransformerChain<KeyToValueMappingTransformer>> _trainingPipeline;
        private static EstimatorChain<KeyToValueMappingTransformer> _trainer;
        private static IDataView _data;

        private static bool  bUrlRequest = false;

        private static string pSql = "",
            sCatalog = "", 
            sTrainer = "", 
            sDatabase = "";

        private static int iLabel = -1;


        protected void Page_Load(object sender, EventArgs e)
        {
            _mlContext = null;
            _model = null;
            _trainingPipeline = null;
            _trainer = null;
            _data = null;

            if (!IsPostBack)
            {   
                if (!String.IsNullOrEmpty(Request.QueryString["pSql"]) && !String.IsNullOrEmpty(Request.QueryString["pLab"]))
                {
                    // Query string value is there so now use it
                    pSql = Convert.ToString(Request.QueryString["pSql"]);
                    iLabel = Convert.ToInt32(Request.QueryString["pLab"]);
                }

                if (pSql != "") 
                {
                    if (iLabel == 0 || iLabel == 1)
                    {
                        Debug.Print("pSql: " + pSql + ", pLab: " + iLabel.ToString());

                        bUrlRequest = true;

                        // Get setup state and trinign data file state
                        GetSetup();

                        // Call Use Model WithS ingleItem
                        UseModelWithSingleItem(txtResult, pSql);

                        // Auto close pages if want to test a lots url requets 
                        //ClosePages();
                    }
                    else
                    {
                        iLabel = -1;
                        bUrlRequest = false;
                    }
                }
                else
                {
                    bUrlRequest = false;
                }
            }
            else // Input fields and submit button used on site, so we haven't label value
            {
                iLabel = -1;
            }
        }
       

        private void GetSetup()
        {
            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();

            DataSet dsINFO = da.SP_RetrieveData("SP_READ_SETUP", sParams);

            if (dsINFO.Tables.Count > 0)
            {
                foreach (DataTable tableINFO in dsINFO.Tables)
                {
                    foreach (DataRow rowINFO in tableINFO.Rows)
                    {
                        sCatalog = (rowINFO["CatalogName"].ToString());
                        sTrainer = (rowINFO["TrainerName"].ToString());
                        sDatabase = (rowINFO["DatabaseName"].ToString());
                    }
                }

                SelectDatabase();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Couldn't read database !');", true);
            }
        }

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
            else if (sDatabase == "project_dataset")
            {
                ExportTextFile("SP_READ_PROJECT");
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

        }

        private void UseModelWithSingleItem(TextBox txtResult, String sTestSql)
        {
            // Create Predictio nEngine1
            //PredictionEngine<SqlData, SqlPrediction> predictionFunction = mlContext.Model.CreatePredictionEngine<SqlData, SqlPrediction>(_model);

            var predictor = _mlContext.Model.CreatePredictionEngine<AppInput, AppPrediction>(_model);


            // Create Issue
            AppInput issueSql = new AppInput
            {
                Text = sTestSql
            };

            // Predict
            var resultPrediction = predictor.Predict(issueSql);

            Logs lo = new Logs();

            if (bUrlRequest)
            {
                int iLog = lo.RecordLogFromURL(sCatalog, sTrainer, sDatabase, sTestSql, iLabel, Convert.ToInt32(resultPrediction.Label));
            }
            else
            {
                int iLog = lo.RecordLogFromInput(sCatalog, sTrainer, sDatabase, sTestSql, Convert.ToInt32(resultPrediction.Label));
            }
            

            // Output Prediction
            txtResult.Text += Environment.NewLine + "=============== Prediction Test of model with a single sample and test dataset ===============" + Environment.NewLine;

            txtResult.Text += $"SQL Command: {sTestSql}" + Environment.NewLine +
                $"Prediction: {((resultPrediction.Label == "1") ? "Positive" : "Negative")}" + Environment.NewLine;

            txtResult.Text += "=============== End of Predictions ===============" + Environment.NewLine;            
        }

        private void ClosePages()
        {
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "CloseTab();", true);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";

            GetSetup();
            
            String sTestSatement = txtInput.Text;

            // Call Use Model WithS ingleItem
            UseModelWithSingleItem(txtResult, sTestSatement);
        }
    }
}