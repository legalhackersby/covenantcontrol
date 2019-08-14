using ml;
using ml.models;
using System;
using System.IO;
using Xunit;

namespace tests
{
    public class CovenantDetectorMLTests
    {
        private string AppPath => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName);
        private readonly CovenantDetectorML covenantDetectorML;

        public CovenantDetectorMLTests()
        {
            var path = Path.Combine(AppPath, "data", "ml", "trainingSets", "covenants", string.Format("{0}{1}", "TestTrainCollection_", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));

            this.covenantDetectorML = new CovenantDetectorML(new Context
            {
                TrainDataPath = path
            });
        }

        [Fact]
        public void Train_Predict_Validate()
        {
            // ASSSET.

            // ACT.
            var a = covenantDetectorML.IsCovenant("Я ковенанта!");
            var b = covenantDetectorML.IsCovenant("согласовывать с арендодателем");
            var c = covenantDetectorML.IsCovenant("СОГЛАСОВЫВАТЬ С АРЕНДОДАТЕЛЕМ!!!");

            // ARRANGE. 
            Assert.False(a);
            Assert.True(b);
            Assert.True(c);
        }
    }
}
