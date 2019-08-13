using Microsoft.ML.Data;

namespace ml.DataStructures
{
    public class CovenantPrediction
    {
        [ColumnName(CovenantDetectorML.PredictedLabelName)]
        public string IsCovenant { get; set; }
    }
}
