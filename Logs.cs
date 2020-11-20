using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace MLFSQLIAD
{
    public class Logs
    {
        public Logs()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int RecordLogFromURL(string sCatalog, string sTrainer, string sDatabase, string sSqlCommand, int iLabel, int iPrediction)
        {
            try
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();

                sParams.Add("@Catalog:" + sCatalog);
                sParams.Add("@Trainer:" + sTrainer);
                sParams.Add("@Database:" + sDatabase);
                sParams.Add("@SqlCommand:" + sSqlCommand);
                sParams.Add("@Label:" + iLabel);
                sParams.Add("@Prediction:" + iPrediction);

                int id = da.InsertRecord("SP_INSERT_LOG_FROM_URL", sParams);

                return id;

            }
            catch
            {
                return -1;
            }
        }

        public int RecordLogFromInput(string sCatalog, string sTrainer, string sDatabase, string sSqlCommand, int iPrediction)
        {
            try
            {
                Data da = new Data();
                List<string> sParams = new List<string>();
                sParams.Clear();

                sParams.Add("@Catalog:" + sCatalog);
                sParams.Add("@Trainer:" + sTrainer);
                sParams.Add("@Database:" + sDatabase);
                sParams.Add("@SqlCommand:" + sSqlCommand);
                sParams.Add("@Prediction:" + iPrediction);

                int id = da.InsertRecord("SP_INSERT_LOG_FROM_INPUT", sParams);

                return id;

            }
            catch
            {
                return -1;
            }
        }
    }
}