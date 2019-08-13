using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using ml.DataStructures;
using ml.models;

namespace ml
{
    public class CovenantDetectorML
    {
        public const string PredictedLabelName = "PredictedLabel";
        private string AppPath => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName);
        private string DefaultCovenantDataPath => Path.Combine(AppPath, "data", "ml", "trainingSets", "covenants", "CovenantsCollection");
        private string DefaultNonCovenantDataPath => Path.Combine(AppPath, "data", "ml", "trainingSets", "covenants", "NonCovenantsCollection");
        private string DefaultTrainDataPath => Path.Combine(AppPath, "data", "ml", "trainingSets", "covenants", "TrainCollection");
        private PredictionEngine<CovenantInput, CovenantPrediction> Predictor { get; set; }

        public CovenantDetectorML(Context context)
        {
            // Create the dataset if it doesn't exists.
            if (!File.Exists(DefaultTrainDataPath))
            {
                this.CreateTrainDataStructure(context);
            }

            this.Init(context);
        }

        private void Init(Context context)
        {
            var mlContext = new MLContext();

            var data = mlContext.Data.LoadFromTextFile<CovenantInput>(
                path: context.TrainDataPath ?? DefaultTrainDataPath,
                hasHeader: true,
                separatorChar: '\t');

            var labelName = nameof(CovenantInput.Label);
            var textName = nameof(CovenantInput.Text);

            var dataProcessPipeline = mlContext
                .Transforms
                .Conversion
                .MapValueToKey(labelName, labelName)
                .Append(mlContext.Transforms.Text.FeaturizeText("FeaturesText", new TextFeaturizingEstimator.Options
                {
                    WordFeatureExtractor = new WordBagEstimator.Options
                    {
                        NgramLength = 2,
                        UseAllLengths = true
                    },
                    CharFeatureExtractor = new WordBagEstimator.Options
                    {
                        NgramLength = 3,
                        UseAllLengths = false
                    }
                }, textName))
                .Append(mlContext.Transforms.CopyColumns("Features", "FeaturesText"))
                .Append(mlContext.Transforms.NormalizeLpNorm("Features", "Features"))
                .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm. (Averaged Perceptron (linear learner))
            var trainer = mlContext
                .MulticlassClassification
                .Trainers
                .OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron(
                    labelColumnName: labelName,
                    numberOfIterations: 10,
                    featureColumnName: "Features"),
                    labelColumnName: labelName)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(CovenantDetectorML.PredictedLabelName, CovenantDetectorML.PredictedLabelName));

            var trainingPipeLine = dataProcessPipeline.Append(trainer);

            mlContext.MulticlassClassification.CrossValidate(
                data: data,
                estimator: trainingPipeLine,
                numberOfFolds: 5);

            // Train a model on the full dataset to help us get better results.
            var model = trainingPipeLine.Fit(data);

            this.Predictor = mlContext.Model.CreatePredictionEngine<CovenantInput, CovenantPrediction>(model);
        }

        public bool IsCovenant(string input)
        {
            var prediction = this.Predictor.Predict(new CovenantInput { Text = input });

            return prediction.IsCovenant == "covenant";
        }

        private void CreateTrainDataStructure(Context context)
        {
            string[] nonCovenants = File.ReadAllLines(context.NonCovenantDataPath ?? DefaultNonCovenantDataPath);
            string[] covenants = File.ReadAllLines(context.CovenantDataPath ?? DefaultCovenantDataPath);

            if (!Directory.Exists(Path.GetDirectoryName(context.TrainDataPath ?? DefaultTrainDataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(context.TrainDataPath ?? DefaultTrainDataPath));
            }

            using (var fs = File.Create(context.TrainDataPath ?? DefaultTrainDataPath))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                int lineNum = 0;
                while (lineNum < nonCovenants.Length || lineNum < covenants.Length)
                {
                    if (lineNum < nonCovenants.Length && !string.IsNullOrWhiteSpace(nonCovenants[lineNum]))
                    {
                        writer.WriteLine($"nonCovenant\t{nonCovenants[lineNum]}");
                    }

                    if (lineNum < covenants.Count())
                    {
                        writer.WriteLine($"covenant\t{covenants[lineNum]}");
                    }

                    lineNum++;
                }
            }
        }
    }
}
