using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System;
using System.Text;

namespace MLFSQLIAD
{
    internal class HtmlTables
    {       

        public HtmlTables()
        {
        }

        public bool SP_populateHtmlTables(Panel pnlID, string sTableID, string sStoredProcedure,
            List<string> lColumnNames, List<string> lDBFieldNames, List<string> sParams)
        {
            Data da = new Data();
            bool bHasrow = false;
            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProcedure, sParams);

            dt.Load(objDataReader);

            if (dt.Rows.Count > 0)
            {

                StringBuilder sb = new StringBuilder();

                sb.Append("<table id='" + sTableID + "' class='table table-striped table-bordered' BorderStyle='Solid' BorderWidth='2'><thead><tr>");

                for (int i = 0; i < lColumnNames.Count; i++)
                {
                    sb.Append("<th class='input-filter' style='text-align:center;'>" + lColumnNames[i] + "</th>");

                }

                sb.Append("</tr></thead><tbody>");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    sb.Append("<tr>");


                    foreach (string name in lDBFieldNames)
                    {
                        sb.Append("<td style='text-align:center'>" + dt.Rows[i][name].ToString() + "</td>");
                    }

                    //sb.Append("<td style='text-align:center'>" + dt.Rows[i]["COMPLETE"].ToString() + "</td><td style='text-align:center'>" + dt.Rows[i]["DATE_CREATED"].ToString() + "</td><td style='text-align:center'>" + dt.Rows[i]["DATE_MODIFIED"].ToString() + "</td>");

                    //hyperlink for details page and Lagan view
                    sb.Append("<td style='text-align:center'><a class='btn btn-info btn-xs' onclick=btnEdit_Click(this)><i class='fas fa-pencil-alt'></i></a>" + 
                        " <a class='btn btn-danger btn-xs' onclick=btnDelete_Click(this)><i class='fas fa-trash'></i></a></td></tr>");

                }

                sb.Append("</tbody><tfoot><tr>");

                for (int i = 0; i < lColumnNames.Count - 1; i++)
                {
                    sb.Append("<th>" + lColumnNames[i] + "</th>");
                }

                sb.Append("<th></th></tr></tfoot></table>");

                pnlID.Controls.Add(new LiteralControl(sb.ToString()));

                bHasrow = true;
            }
            else
            {
                bHasrow = false;
            }

            return bHasrow;
        }

        public bool SP_populateHistoryTables(Panel pnlID, string sTableID, string sStoredProcedure,
            List<string> lColumnNames, List<string> lDBFieldNames, List<string> sParams)
        {
            Data da = new Data();
            bool bHasrow = false;
            DataTable dt = new DataTable();
            SqlDataReader objDataReader = da.ExecuteReader(sStoredProcedure, sParams);

            dt.Load(objDataReader);

            if (dt.Rows.Count > 0)
            {

                StringBuilder sb = new StringBuilder();

                sb.Append("<table id='" + sTableID + "' class='table table-striped table-bordered' BorderStyle='Solid' BorderWidth='2'><thead><tr>");

                for (int i = 0; i < lColumnNames.Count; i++)
                {
                    sb.Append("<th class='input-filter' style='text-align:center;'>" + lColumnNames[i] + "</th>");

                }

                sb.Append("</tr></thead><tbody>");

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    sb.Append("<tr>");


                    foreach (string name in lDBFieldNames)
                    {
                        sb.Append("<td style='text-align:center'>" + dt.Rows[i][name].ToString() + "</td>");
                    }

                    sb.Append("</tr>");
                }

                sb.Append("</tbody><tfoot><tr>");

                for (int i = 0; i < lColumnNames.Count - 1; i++)
                {
                    sb.Append("<th>" + lColumnNames[i] + "</th>");
                }

                sb.Append("<th></th></tr></tfoot></table>");

                pnlID.Controls.Add(new LiteralControl(sb.ToString()));

                bHasrow = true;
            }
            else
            {
                bHasrow = false;
            }

            return bHasrow;
        }
    }
}