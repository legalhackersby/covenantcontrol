using Microsoft.ML.Data;

namespace ml.DataStructures
{
    public class CovenantInput
    {
        [LoadColumn(0)]
        public string Label { get; set; }

        [LoadColumn(1)]
        public string Text { get; set; }
    }
}
