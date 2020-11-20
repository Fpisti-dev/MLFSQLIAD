using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace MLFSQLIAD
{
    public class Chart
    {
        /***************************************************************************************
        * Goggle API must be insert to page heading section
        * <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        * 
        * A lot of other option parameters available to charts. If you want to add someting else 
        * please visit: https://developers.google.com/chart/
        * 
        *****************************************************************************************/
        Data da = new Data();
          
        public Chart()
        {
        }

        /*********************************************
        * 
        * Call SP to populate Basic Google Chart
        * 
        * Use for:
        * -AreaChart
        * -BarChart
        * -BubbleChart
        * -ColumnChart
        * -LineChart
        * -ScatterChart
        * -SteppedAreaChart
        * 
        **********************************************/
        public void SP_populateBasicGoogleChart(Label lblInfo, Literal ltChart, string sChartPanelID, Literal ltMetrics, string sMetricsPanelID, string sStoredProc, string sChartType,
            int iWidth, int iHeight, string sTableName, string shAxisName, string shAxisNameColor,
            string svAxisName, string svAxisNameColor, string[] aColumnNames, string[] aDBFieldNames,
            List<string> sParams)
        {
            try
            {
                DataTable dt = new DataTable();


                SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

                dt.Load(objDataReader);

                lblInfo.Text = "Trainer: " + dt.Rows[0]["TrainerName"].ToString() + ", Database: " + dt.Rows[0]["DatabaseName"].ToString();

                // Create Metrics table
                metricsTable(ltMetrics, "MainContent_pnlMetrics", "true", "50%", "100%",
                    Convert.ToInt32(dt.Rows[0]["True Postivive"]),
                    Convert.ToInt32(dt.Rows[0]["False Postivive"]),
                    Convert.ToInt32(dt.Rows[0]["True Negative"]),
                    Convert.ToInt32(dt.Rows[0]["False Negative"]));

                StringBuilder str = new StringBuilder();

                str.Append(@"<script type=text/javascript> google.charts.load('current', {packages: ['corechart', 'bar']});
                        google.charts.setOnLoadCallback(drawChart);
                        function drawChart() {
                        var data = new google.visualization.DataTable();");
                str.Append("data.addColumn('string', '" + shAxisName + "');");

                
                for (int i = 0; i < aColumnNames.Length; i++)
                {
                    str.Append("data.addColumn('number', '" + aColumnNames[i] + "');");
                }

                // sample format    ['Year', 'Sales', 'Expenses'],
                //                  ['2013',  1000,      400],
                str.Append("data.addRows(" + dt.Rows.Count + ");");

                
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < aDBFieldNames.Length; j++)
                    {
                        str.Append("data.setValue(" + i + "," + j + ",'" + dt.Rows[i][aDBFieldNames[j]].ToString() + "') ;");
                    }
                }

                str.Append(" var chart = new google.visualization." + sChartType + "(document.getElementById('" + sChartPanelID + "'));");

                // Click listeners for bars
                //str.Append("google.visualization.events.addListener(chart, 'select', function () { " +
                //    "var selection = chart.getSelection(); if (selection.length) { displayDetails(selection);  } });");

                str.Append(" chart.draw(data, {title: '', titleTextStyle: { fontSize: 12}, legend: { textStyle: { fontSize: 12}}, chartArea: {  width: '60%', height: '70%' }, ");
                str.Append("hAxis: {title: '" + shAxisName + "', titleTextStyle: {color: '" + shAxisNameColor + "'}},");
                str.Append("vAxis: {title: '" + svAxisName + "', titleTextStyle: {color: '" + svAxisNameColor + "'}}");
                str.Append("}); }");
                str.Append("</script>");
                ltChart.Text = str.ToString();

                objDataReader.Close();
            }
            catch
            {

            }
        }

        /*********************************************
        * 
        * Call SP to populate Single Value Google Chart
        * 
        * Use for:
        * -DonutChart
        * -PieChart
        * -3D PieChart
        *     
        * *********************************************/
        public void SP_populateSingleValueGoogleChart(Label lblInfo, Literal ltChart, string sChartPanelID, Literal ltMetrics, string sMetricsPanelID, string sStoredProc, string sChartType,
        int iWidth, int iHeight, string sTableName, string[] aColumnNames, string[] aDBFieldNames,
        List<string> sParams)
        {
            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

            dt.Load(objDataReader);

            lblInfo.Text = "Trainer: " + dt.Rows[0]["TrainerName"].ToString() + ", Database: " + dt.Rows[0]["DatabaseName"].ToString();

            // Create Metrics table
            metricsTable(ltMetrics, "MainContent_pnlMetrics", "true", "50%", "100%",
                Convert.ToInt32(dt.Rows[0]["True Postivive"]),
                Convert.ToInt32(dt.Rows[0]["False Postivive"]),
                Convert.ToInt32(dt.Rows[0]["True Negative"]),
                Convert.ToInt32(dt.Rows[0]["False Negative"]));

            if (dt.Rows.Count > 0 )
            {
                StringBuilder str = new StringBuilder();

                str.Append(@"<script type=text/javascript> google.charts.load(*current*, {packages:[*corechart*]});
                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                    var data = new google.visualization.arrayToDataTable([");

                str.Append("['" + aColumnNames[0] + "', '" + aColumnNames[1] + "']");

                // sample format ['Task', 'Hours per Day'],
                //               ['Work',     11],
                for (int i = 0; i <= aDBFieldNames.Length - 1; i++)
                {
                    str.Append(",['" + aDBFieldNames[i].ToString() + "'," +
                        dt.Rows[0][aDBFieldNames[i]].ToString() + "]");
                }

                //sTableName += " for " + dt.Rows[0]["DatabaseName"].ToString();

                str.Append("]); var chart = new google.visualization.PieChart(document.getElementById('" +
                    sChartPanelID + "'));");

                if (sChartType == "PieChart")
                {
                    str.Append("var options = {width: " + iWidth + ", height: " + iHeight + ", title: '" +
                        sTableName + "'};");
                }
                else if (sChartType == "DonutChart")
                {
                    str.Append("var options = {width: " + iWidth + ", height: " + iHeight + ", title: '" + sTableName +
                        "', pieHole: 0.4};"); // set hole manually if you want a different size of hole
                }
                else if (sChartType == "3D PieChart")
                {
                    str.Append("var options = {width: " + iWidth + ", height: " + iHeight + ", title: '" + sTableName +
                        "', is3D: true};");
                }

                str.Append(" chart.draw(data, options);");
                str.Append("}</script>");
                ltChart.Text = str.ToString().TrimEnd(',').Replace('*', '"');
            }

            objDataReader.Close();
        }
      

        /*********************************************
        * 
        * Call SP to populate Gauge Google Charts
        *     
        * *********************************************/
        public void SP_populateGaugeGoogleChart(Label lblInfo, Literal ltChart, string sChartPanelID, Literal ltMetrics, string sMetricsPanelID, string sStoredProc, string sChartType,
            int iWidth, int iHeight, int iRedFrom, int iRedTo, int iYellowFrom, int iYellowTo, int iMinorTicks,
            string[] aColumnNames, string[] aDBFieldNames, List<string> sParams)
        {
            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

            dt.Load(objDataReader);

            lblInfo.Text = "Trainer: " + dt.Rows[0]["TrainerName"].ToString() + ", Database: " + dt.Rows[0]["DatabaseName"].ToString();

            // Create Metrics table
            metricsTable(ltMetrics, "MainContent_pnlMetrics", "true", "50%", "100%",
                Convert.ToInt32(dt.Rows[0]["True Postivive"]),
                Convert.ToInt32(dt.Rows[0]["False Postivive"]),
                Convert.ToInt32(dt.Rows[0]["True Negative"]),
                Convert.ToInt32(dt.Rows[0]["False Negative"]));

            StringBuilder str = new StringBuilder();

            str.Append(@"<script type=text/javascript> google.charts.load('current', {packages:['gauge']});
                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                    var data = new google.visualization.arrayToDataTable([");

            str.Append("['" + aColumnNames[0] + "', '" + aColumnNames[1] + "']");

            // sample format   ['Label', 'Value'],
            //                 ['Memory', 80],
            for (int i = 0; i <= aDBFieldNames.Length - 1; i++)
            {
                str.Append(",['" + aDBFieldNames[i].ToString() + "'," +
                    dt.Rows[0][aDBFieldNames[i]].ToString() + "]");
            }

            str.Append("]); var chart = new google.visualization." + sChartType + "(document.getElementById('" +
                sChartPanelID + "'));");

            str.Append("var options = {width: " + iWidth + ", height: " + iHeight +
                ", redFrom: " + iRedFrom + ", redTo: " + iRedTo + ", max: " + iRedTo +
                ", yellowFrom: " + iYellowFrom + ", yellowTo: " + iYellowTo +
                ", minorTicks: " + iMinorTicks + " };");


            str.Append(" chart.draw(data, options);");
            str.Append("}</script>");
            ltChart.Text = str.ToString();

            objDataReader.Close();
        }
  

        /*********************************************
        * 
        * Call SP to populate Table
        *     
        * *********************************************/
        public void SP_populateTable(Label lblInfo, Literal ltChart, string sChartPanelID, Literal ltMetrics, string sMetricsPanelID, string sStoredProc, string sChartType,
            string sShowRowNumber, string sWidth, string sHeight, string[] aColumnNames, string[] aDBFieldNames, List<string> sParams)
        {
            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

            dt.Load(objDataReader);

            if (dt.Rows.Count > 0 )
            {
                lblInfo.Text = "Trainer: " + dt.Rows[0]["TrainerName"].ToString() + ", Database: " + dt.Rows[0]["DatabaseName"].ToString();

                // Create Metrics table
                metricsTable(ltMetrics, "MainContent_pnlMetrics", "true", "50%", "100%", 
                    Convert.ToInt32(dt.Rows[0]["True Postivive"]),
                    Convert.ToInt32(dt.Rows[0]["False Postivive"]),
                    Convert.ToInt32(dt.Rows[0]["True Negative"]),
                    Convert.ToInt32(dt.Rows[0]["False Negative"]));

                StringBuilder str = new StringBuilder();

                str.Append(@"<script type=text/javascript> google.charts.load('current', {packages:['table']});
                    google.charts.setOnLoadCallback(drawTable);
                    function drawTable() {
                    var data = new google.visualization.DataTable();");

                str.Append("data.addColumn('string', '" + aColumnNames[0] + "');");
                str.Append("data.addColumn('number', '" + aColumnNames[1] + "');");
                str.Append("data.addRows([");

                // sample format    ['Mike',  {v: 10000, f: '$10,000'}, true],
                for (int i = 0; i <= aDBFieldNames.Length - 1; i++)
                {
                    str.Append("['" + aDBFieldNames[i].ToString() + "'," +
                        dt.Rows[0][aDBFieldNames[i]].ToString());
                    if (i < aDBFieldNames.Length - 1)
                    {
                        str.Append(" ],");
                    }
                    else
                    {
                        str.Append(" ]]);");
                    }
                }

                str.Append("var table = new google.visualization." + sChartType + "(document.getElementById('" +
                    sChartPanelID + "'));");

                str.Append("var options = {showRowNumber: " + sShowRowNumber +
                    ", width: '" + sWidth + "', height: '" + sHeight + "'};");

                str.Append(" table.draw(data, options);");
                str.Append("}</script>");
                ltChart.Text = str.ToString();
            }            

            objDataReader.Close();
        }

        /*********************************************
        * 
        * Create Metrics Table
        *     
        * *********************************************/
        private void metricsTable(Literal ltMetrics, string sMetricsPanelID,  string sShowRowNumber, string sWidth, 
            string sHeight, int iTP, int iFP, int iTN,  int iFN)
        {

            double dAccuracy = (double)(iTP + iTN) / (iTP + iFP + iTN + iFN);
            double dPrecision = (double)iTP / (iTP + iFP);
            double dRecall = (double)iTP / (iTP + iFN);
            double dF1 = 2 * ((dPrecision * dRecall) / (dPrecision + dRecall));


            StringBuilder str = new StringBuilder();

            str.Append(@"<script type=text/javascript> google.charts.load('current', {packages:['table']});
                    google.charts.setOnLoadCallback(drawTable);
                    function drawTable() {
                    var data = new google.visualization.DataTable();");

            str.Append("data.addColumn('string', 'Metrics');");
            str.Append("data.addColumn('string', 'Value');");
            str.Append("data.addRows([");

            str.Append("['Accuracy','" + Math.Round(dAccuracy, 4) + "' ],");
            str.Append("['Precision','" + Math.Round(dPrecision, 4) + "' ],");
            str.Append("['Recall','" + Math.Round(dRecall, 4) + "' ],");
            str.Append("['F1 Score','" + Math.Round(dF1,4) + "' ]]); ");
           

            str.Append("var table = new google.visualization.Table(document.getElementById('" +
                sMetricsPanelID + "'));");

            str.Append("var options = {showRowNumber: " + sShowRowNumber +
                ", width: '" + sWidth + "', height: '" + sHeight + "'};");

            str.Append(" table.draw(data, options);");
            str.Append("}</script>");
            ltMetrics.Text = str.ToString();
        }
    }
}