using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MLFSQLIAD
{
    public class Data
    {

        // MDF File Datbase
        public static string sConnection = ConfigurationManager.ConnectionStrings["MLConn"].ConnectionString;

        public Data()
        {
        }

        public SqlDataReader ExecuteReader(string sStoredProc, List<string> sParams)
        {
            SqlConnection objConn = new SqlConnection(sConnection);

            using (SqlCommand objCommand = new SqlCommand(sStoredProc, objConn))
            {
                objConn.Open();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandTimeout = 0;

                foreach (string myparam in sParams)
                {
                    string sName = "";
                    string sValue = "";
                    sName = myparam.Substring(0, myparam.IndexOf(":"));
                    sValue = myparam.Substring(myparam.IndexOf(":") + 1);

                    objCommand.Parameters.Add(sName, SqlDbType.VarChar).Value = sValue;
                }

                SqlDataReader reader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        public void SP_CreateUpdateRecord(string sStoredProc, List<string> sParams)
        {
            SqlConnection objConn = new SqlConnection(sConnection);
            objConn.Open();

            SqlCommand objCommand = new SqlCommand(sStoredProc, objConn);
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.Parameters.Clear();

            foreach (string myparam in sParams)
            {
                string sName = "";
                string sValue = "";
                sName = myparam.Substring(0, myparam.IndexOf(":"));
                sValue = myparam.Substring(myparam.IndexOf(":") + 1);

                objCommand.Parameters.Add(sName, SqlDbType.VarChar).Value = sValue;
            }

            objCommand.ExecuteNonQuery();

            objCommand.Parameters.Clear();
            objCommand.Dispose();
            objConn.Close();
        }

        public String SP_RetrieveDataOneValue(string sStoredProc, List<string> sParams)
        {
            //create dataset
            //  DataSet ds = new DataSet();
            //  ds.Tables.Add(new DataTable());

            string sReturnString = "";

            SqlDataReader objDataReader = ExecuteReader(sStoredProc, sParams);

            if (objDataReader.HasRows)
            {
                while (objDataReader.Read())
                {
                    sReturnString = objDataReader[0].ToString();
                }
            }

            return sReturnString;
        }

        public DataSet SP_RetrieveData(string sStoredProc, List<string> sParams)
        {
            //create dataset
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());

            SqlDataReader objDataReader = ExecuteReader( sStoredProc, sParams);

            if (objDataReader.HasRows)
            {
                for (int k = 0; k < objDataReader.FieldCount; k++)
                {
                    ds.Tables[0].Columns.Add(new DataColumn(objDataReader.GetName(k).ToString()));
                }

                int iRowCount = 1;
                while (objDataReader.Read())
                {
                    DataRow dr = ds.Tables[0].NewRow();

                    for (int colcount = 0; colcount < objDataReader.FieldCount; colcount++)
                    {
                        dr[colcount] = objDataReader[colcount].ToString();
                    }

                    ds.Tables[0].Rows.Add(dr);
                    iRowCount++;
                }
            }

            return ds;
        }

        public int InsertRecord(string sStoredProc, List<string> sParams)
        {
            int ID = 0;

            try
            {
                using (SqlConnection objConnection = new SqlConnection(sConnection))
                {
                    SqlCommand objCommand = new SqlCommand(sStoredProc, objConnection);
                    objCommand.Connection.Open();
                    objCommand.CommandType = CommandType.StoredProcedure;

                    objCommand.Parameters.Clear();

                    foreach (string myparam in sParams)
                    {
                        string sName = "";
                        string sValue = "";
                        sName = myparam.Substring(0, myparam.IndexOf(":"));
                        sValue = myparam.Substring(myparam.IndexOf(":") + 1);

                        objCommand.Parameters.Add(sName, SqlDbType.VarChar).Value = sValue;

                    }

                    var returnParameter = objCommand.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    objCommand.ExecuteNonQuery();
                    objCommand.Parameters.Clear();
                    ID = System.Convert.ToInt32(returnParameter.Value);

                }
            }
            catch
            {
                ID = -1;
            }

            return ID;
        }
    }
}