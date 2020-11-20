using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MLFSQLIAD
{
    public partial class Reports : System.Web.UI.Page
    {
        private static string sChartType = "ColumnChart";

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateTables();

            if (!IsPostBack)
            {
                populateDropDownFromArray();
                GetLastTestNumber();
            }
        }

        private void GetLastTestNumber()
        {
            Data da = new Data();
            List<string> sParams = new List<string>();
            sParams.Clear();

            string sLastNumber = da.SP_RetrieveDataOneValue("SP_READ_LAST_TEST_NUMBER", sParams);
             
            if (sLastNumber != "")
            {
                txtTestNumber.Text = (Convert.ToInt32(sLastNumber)).ToString();
                RangeValidator1.MaximumValue = sLastNumber;
            }
            
        }

        private void PopulateTables()
        {
            // Clear panels first
            pnlTest.Controls.Clear(); 

            // array for column names
            List<string> lColumnNames = new List<string>();
            lColumnNames.Clear();
            lColumnNames.Add("ID");
            lColumnNames.Add("Test No.");
            lColumnNames.Add("Trainer Name");
            lColumnNames.Add("Database Name");
            lColumnNames.Add("SQL Command");
            lColumnNames.Add("Label");
            lColumnNames.Add("Prediction");
            lColumnNames.Add("Recorded At");

            // database column names
            List<string> lDBFieldNames = new List<string>();
            lDBFieldNames.Clear();
            lDBFieldNames.Add("Id");
            lDBFieldNames.Add("TestNumber");
            lDBFieldNames.Add("TrainerName");
            lDBFieldNames.Add("DatabaseName");
            lDBFieldNames.Add("SqlCommand");
            lDBFieldNames.Add("Label");
            lDBFieldNames.Add("Prediction");
            lDBFieldNames.Add("Recorded");


            List<string> sParams = new List<string>();
            sParams.Clear();
            //sParams.Add("@Form:" + sFormName);

            HtmlTables ht = new HtmlTables();
            bool bHasrow = ht.SP_populateHistoryTables(pnlTest, "test_table", "SP_READ_HISTORY", lColumnNames, lDBFieldNames, sParams);

            pnlURL.Controls.Clear();

            lColumnNames.Clear();
            lColumnNames.Add("ID");
            lColumnNames.Add("Trainer Name");
            lColumnNames.Add("Database Name");
            lColumnNames.Add("SQL Command");
            lColumnNames.Add("Label");
            lColumnNames.Add("Prediction");
            lColumnNames.Add("Recorded At");

            // database column names
            lDBFieldNames.Clear();
            lDBFieldNames.Add("Id");
            lDBFieldNames.Add("TrainerName");
            lDBFieldNames.Add("DatabaseName");
            lDBFieldNames.Add("SqlCommand");
            lDBFieldNames.Add("Label");
            lDBFieldNames.Add("Prediction");
            lDBFieldNames.Add("Recorded");


            sParams.Clear();
            //sParams.Add("@Form:" + sFormName);

            bHasrow = ht.SP_populateHistoryTables(pnlURL, "url_table", "SP_READ_HISTORY_FROM_URL", lColumnNames, lDBFieldNames, sParams);

            pnlInput.Controls.Clear();

            lColumnNames.Clear();
            lColumnNames.Add("ID");
            lColumnNames.Add("Trainer Name");
            lColumnNames.Add("Database Name");
            lColumnNames.Add("SQL Command");
            lColumnNames.Add("Prediction");
            lColumnNames.Add("Recorded At");

            // database column names
            lDBFieldNames.Clear();
            lDBFieldNames.Add("Id");
            lDBFieldNames.Add("TrainerName");
            lDBFieldNames.Add("DatabaseName");
            lDBFieldNames.Add("SqlCommand");
            lDBFieldNames.Add("Prediction");
            lDBFieldNames.Add("Recorded");


            sParams.Clear();
            //sParams.Add("@Form:" + sFormName);

            bHasrow = ht.SP_populateHistoryTables(pnlInput, "input_table", "SP_READ_HISTORY_FROM_INPUT", lColumnNames, lDBFieldNames, sParams);

        }

        private void populateDropDownFromArray()
        {

            string[] aChartNames = new string[] { "3D PieChart", "BarChart", "ColumnChart", "DonutChart", "Gauge",  "PieChart", "SteppedAreaChart", "Table" };

            List<System.Web.UI.WebControls.ListItem> chartTypes = new List<System.Web.UI.WebControls.ListItem>();

            chartTypes.Add(new ListItem("-- Please select --"));

            for (int i = 0; i < aChartNames.Length; i++)
            {
                chartTypes.Add(new ListItem(aChartNames[i]));
            }

            ddlCharType.DataSource = chartTypes;
            ddlCharType.DataBind();
        }

        protected void ddlCharType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltChart.Text = "";
            ltMetrics.Text = "";

            pnlChart.Controls.Clear();

            if (ddlCharType.SelectedIndex > 0)
            {
                sChartType = ddlCharType.SelectedValue.ToString();

                if (sChartType == "BarChart" || // basic charts
                    sChartType == "ColumnChart" ||
                    sChartType == "SteppedAreaChart")
                {
                    displayBasicCharts();
                }
                else if (sChartType == "DonutChart" ||
                    sChartType == "PieChart" ||
                    sChartType == "3D PieChart")
                {
                    displaySingleValueChart();
                }
                else if (sChartType == "Gauge") { displayGaugeChart(); }
                else if (sChartType == "Table") { displayTable(); }
            }
            
        }

        private void displayBasicCharts()
        {
            // names for bars
            string[] aColumnNames = new string[] { "True Postivive", "False Postivive", "True Negative", "False Negative" };

            // database column names, first usually hold a string for x-axis, and others hold a number for bars 
            string[] aDBFieldNames = new string[] { "DatabaseName", "True Postivive", "False Postivive", "True Negative", "False Negative" };

            // a list of parameters for stored procedors only!
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@TestNumber:" + txtTestNumber.Text);

            Chart ch = new Chart();

            /***************
             * Parameter's definition
             * 1, Literal ID
             * 2, Element ID for chart load to, div on panel booth usable
             * 3, Stored Procedor name
             * 4, Type of chart, you can choose one from aChartNames below
             * 5-6, Width and heigth
             * 7, Chart title
             * 8, hAxis name
             * 9, hAxis name color
             * 10, vAxis name
             * 11, vAxis name color
             * 12, Array for name of columns
             * 13, Array for database column names
             * 14, A list from Stored Procedors parameters
             ****************/

            ch.SP_populateBasicGoogleChart(lblInfo, ltChart, "MainContent_pnlChart", ltMetrics, "MainContent_pnlMetrics", "SP_READ_HISTORY_CHART", sChartType, 800, 600,
            "", "Groups", "green", "Values", "red", aColumnNames, aDBFieldNames, sParams);
        }

        private void displaySingleValueChart()
        {
            // names for bars
            string[] aColumnNames = new string[] { "Group", "Value" };

            // database column names, first usually hold a string for x-axis, and others hold a number for bars 
            string[] aDBFieldNames = new string[] {"True Postivive", "False Postivive", "True Negative", "False Negative" };

            // a list of parameters for stored procedors only!
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@TestNumber:" + txtTestNumber.Text);

            Chart ch = new Chart();

            /***************
            * Parameter's definition
            * 1, Literal ID
            * 2, Element ID for chart load to, div on panel, booth usable
            * 3, Stored Procedor name
            * 4, Type of chart, you can choose one from aChartNames below
            * 5-6, Width and heigth
            * 7, Chart title
            * 8, Array for name of columns
            * 9, Array for database column names
            * 10, A list from Stored Procedors parameters
            ****************/

            ch.SP_populateSingleValueGoogleChart(lblInfo, ltChart, "MainContent_pnlChart", ltMetrics, "MainContent_pnlMetrics", "SP_READ_HISTORY_CHART", sChartType,
              600, 480, "", aColumnNames, aDBFieldNames, sParams);
        }
 

        private void displayGaugeChart()
        {
            // names for bars
            string[] aColumnNames = new string[] { "Group", "Value" };

            // database column names, first usually hold a string for x-axis, and others hold a number for bars 
            string[] aDBFieldNames = new string[] { "True Postivive", "False Postivive", "True Negative", "False Negative" };

            // a list of parameters for stored procedors only!
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@TestNumber:" + txtTestNumber.Text);

            Chart ch = new Chart();

            /***************
            * Parameter's definition
            * 1, Literal ID
            * 2, Element ID for chart load to, div on panel, booth usable
            * 3, Stored Procedor name
            * 4, Type of chart, you can choose one from aChartNames below
            * 5-11, Width, heigth, redFrom, redTo, yellowFrom, yellowTo, minorTicks      
            * 12, Array for name of columns
            * 13, Array for database column names
            * 14, A list from Stored Procedors parameters
            ****************/

            ch.SP_populateGaugeGoogleChart(lblInfo, ltChart, "MainContent_pnlChart", ltMetrics, "MainContent_pnlMetrics", "SP_READ_HISTORY_CHART", sChartType,
              600, 150, 230, 250, 210, 230, 25, aColumnNames, aDBFieldNames, sParams);
        }

        private void displayTable()
        {
            // names for bars
            string[] aColumnNames = new string[] { "Group", "Value" };

            // database column names, first usually hold a string for x-axis, and others hold a number for bars 
            string[] aDBFieldNames = new string[] { "True Postivive", "False Postivive", "True Negative", "False Negative" };

            // a list of parameters for stored procedors only!
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@TestNumber:" + txtTestNumber.Text);

            Chart ch = new Chart();

            /***************
            * Parameter's definition
            * 1, Literal ID
            * 2, Element ID for chart load to, div on panel, booth usable
            * 3, Stored Procedor name
            * 4, Type of chart, you can choose one from aChartNames below
            * 5-7, showRowNumber, width, height    
            * 8, Array for name of columns
            * 9, Array for database column names
            * 10, A list from Stored Procedors parameters
            ****************/

            ch.SP_populateTable(lblInfo, ltChart, "MainContent_pnlChart", ltMetrics, "MainContent_pnlMetrics", "SP_READ_HISTORY_CHART", sChartType,
               "true", "50%", "100%", aColumnNames, aDBFieldNames, sParams);
        }
    }
}