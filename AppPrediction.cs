using Microsoft.ML.Data;

namespace MLFSQLIAD
{
    public class AppPrediction
    {

        [ColumnName("PredictedLabel")]
        public string Label { get; set; }

    }
}