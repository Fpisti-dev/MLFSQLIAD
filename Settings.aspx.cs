using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MLFSQLIAD
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetSetup();
            }

            PopulateTables();

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
                        rblCatalog.SelectedValue = (rowINFO["CatalogName"].ToString());
                        rblTrainer.SelectedValue = (rowINFO["TrainerName"].ToString());
                        rblDatabase.SelectedValue = (rowINFO["DatabaseName"].ToString());
                    }
                }
            }
        }

        private void PopulateTables()
        {

            // Clear panels first
            pnlBurp.Controls.Clear();
            pnlFuzzy.Controls.Clear();
            pnlOwasp.Controls.Clear();
            pnlProject.Controls.Clear();

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
            bool bHasrow = ht.SP_populateHtmlTables(pnlBurp, "burp_table", "SP_READ_BURP_SETUP", lColumnNames, lDBFieldNames, sParams);

            bHasrow = ht.SP_populateHtmlTables(pnlFuzzy, "fuzzy_table", "SP_READ_FUZZY_SETUP", lColumnNames, lDBFieldNames, sParams);

            bHasrow = ht.SP_populateHtmlTables(pnlOwasp, "owasp_table", "SP_READ_OWASP_SETUP", lColumnNames, lDBFieldNames, sParams);

            bHasrow = ht.SP_populateHtmlTables(pnlProject, "project_table", "SP_READ_PROJECT_SETUP", lColumnNames, lDBFieldNames, sParams);

        }

        protected void btnSaveSetup_Click(object sender, EventArgs e)
        {
            Debug.Print("DB: " + rblDatabase.SelectedValue);

            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@CatalogName:" + rblCatalog.SelectedValue);
            sParams.Add("@TrainerName:" + rblTrainer.SelectedValue);
            sParams.Add("@DatabaseName:" + rblDatabase.SelectedValue);

            da.SP_CreateUpdateRecord("SP_UPDATE_SETUP", sParams);


            //Reload Default Sita to update training file
            Response.Redirect("Default.aspx");
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sDB = hTab.Value;
            string sSP = "";

            if (sDB == "burp")
            {
                sSP = "SP_DELETE_BURP";
            }
            else if (sDB == "fuzzy")
            {
                sSP = "SP_DELETE_FUZZY";
            }
            else if (sDB == "owasp")
            {
                sSP = "SP_DELETE_OWASP";
            }
            else if (sDB == "project")
            {
                sSP = "SP_DELETE_PROJECT";
            }

            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@ID:" + hID.Value);

            int ID = da.InsertRecord(sSP, sParams);

            PopulateTables();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sDB = hTab.Value;
            string sSP = "";

            if (sDB == "burp")
            {
                sSP = "SP_UPDATE_BURP";
            }
            else if (sDB == "fuzzy")
            {
                sSP = "SP_UPDATE_FUZZY";
            }
            else if (sDB == "owasp")
            {
                sSP = "SP_UPDATE_OWASP";
            }
            else if (sDB == "project")
            {
                sSP = "SP_UPDATE_PROJECT";
            }

            if (txtEditSQL.Text != "")
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();
                sParams.Add("@Text:" + txtEditSQL.Text);
                sParams.Add("@Label:" + Convert.ToInt32(ddlEditLabel.SelectedValue));
                sParams.Add("@ID:" + hID.Value);

                da.SP_CreateUpdateRecord(sSP, sParams);
            }

            PopulateTables();
        }


        protected void btnInsert_Click(object sender, EventArgs e)
        {
            string sDB = ddlInserDatabase.SelectedValue;
            string sSP = "";

            if (sDB == "burp")
            {
                sSP = "SP_INSERT_BURP";
            }
            else if (sDB == "fuzzy")
            {
                sSP = "SP_INSERT_FUZZY";
            }
            else if (sDB == "owasp")
            {
                sSP = "SP_INSERT_OWASP";
            }
            else if (sDB == "project")
            {
                sSP = "SP_INSERT_PROJECT";
            }

            if (txtInsertSQL.Text != "")
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();
                sParams.Add("@Text:" + txtInsertSQL.Text);
                sParams.Add("@Label:" + Convert.ToInt32(ddlInsertLabel.SelectedValue));

                int ID = da.InsertRecord(sSP, sParams);
            }

            PopulateTables();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Debug.Print("Add SQL");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "OpenModal();", true);
        }
    }
}