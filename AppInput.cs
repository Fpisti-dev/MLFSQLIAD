using Microsoft.ML.Data;

namespace MLFSQLIAD
{
    public class AppInput
    {
        [LoadColumn(1)]
        public string Label { get; set; }
        [LoadColumn(0)]
        public string Text { get; set; }
    }
}