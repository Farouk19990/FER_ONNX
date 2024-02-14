using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.model
{
    internal class yolov8
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;


        public yolov8()
        {
            _mlContext = new MLContext();
            _model = LoadModel();
        }
        public struct OnnxConfig
        {
            public const string ModelFilePath = "model\\emotion-ferplus-8.onnx";

            public const int ImageWidth = 64;
            public const int ImageHeight = 64;

            public const string InputLayer = "Input3";
            public const string OutputLayer = "Plus692_Output_0";

            public static readonly string[] Labels = {"neutral",
                "happiness",
                "surprise",
                "sadness",
                "anger",
                "disgust",
                "fear",
                "contempt"
            };

            public static float[] Softmax(float[] values)
            {
                var exponentialValues = values.Select(v => Math.Exp(v - values.Max()))
                    .ToArray();

                return exponentialValues.Select(exp => (float)(exp / exponentialValues.Sum()))
                    .ToArray();
            }
        }

        public IEnumerable<OutputModel> DetectEmotionsInImageFiles(string[] imagePaths)
        {

            var imageInputs = imagePaths
                .Select(i => new InputModel()
                {
                    ImageAsBitmap = MLImage.CreateFromFile(i)
                }
                ).ToList();


            float[][] scoredImages = ScoreImageList(imageInputs);


            //Format the score with labels
            var result =
                imagePaths.Select(Path.GetFileName)
                    .Zip(
                        scoredImages,
                        (fileName, probabilities) => WrapModelOutput(probabilities, fileName)
                    );


            return result;
        }

        private float[][] ScoreImageList(List<InputModel> imageInputs)
        {
            // Create an IDataView from the image list
            IDataView imageDataView = _mlContext.Data.LoadFromEnumerable(imageInputs);

            // Transform the IDataView with the model
            IDataView scoredData = _model.Transform(imageDataView);

            // Extract the scores from the output layer
            var scoringValues = scoredData.GetColumn<float[]>(OnnxConfig.OutputLayer);

            // Run the scores through the SoftMax function
            float[][] probabilities;
            try
            {
                probabilities = scoringValues.Select(OnnxConfig.Softmax)
                                         .ToArray();
            }
            catch
            {
                probabilities = Enumerable.Repeat(
                                                    Enumerable.Repeat(0f, 8).ToArray(), 1)
                                          .ToArray();
            }

            return probabilities;
        }

        private static OutputModel WrapModelOutput(float[] probabilities, string filename = null)
        {
            List<(string emotion, float probability)> mergedLabelsWithProbabilities =
                                                          OnnxConfig
                                                          .Labels
                                                          .Zip(
                                                                probabilities,
                                                                (emotion, probability) => (emotion, probability))
                                                          .ToList();

            return new OutputModel()
            {
                Filename = filename,
                EmotionProbabilities = mergedLabelsWithProbabilities
            };
        }

        private ITransformer LoadModel()
        {
            var onnxScorer = _mlContext
                .Transforms
                .ApplyOnnxModel(
                    modelFile: OnnxConfig.ModelFilePath,
                    inputColumnNames: new[] { OnnxConfig.InputLayer },
                    outputColumnNames: new[] { OnnxConfig.OutputLayer }
                );

            var preProcessingPipeline = _mlContext
                .Transforms
                .ConvertToGrayscale(
                    inputColumnName: nameof(InputModel.ImageAsBitmap),
                    outputColumnName: nameof(InputModel.ImageAsBitmap))
                 .Append(_mlContext
                    .Transforms
                    .ResizeImages(
                        inputColumnName: nameof(InputModel.ImageAsBitmap),
                        imageWidth: OnnxConfig.ImageWidth,
                        imageHeight: OnnxConfig.ImageHeight,
                        outputColumnName: nameof(InputModel.ImageAsBitmap)
                    )).Append(_mlContext
                    .Transforms
                    .ExtractPixels(
                        inputColumnName: nameof(InputModel.ImageAsBitmap),
                        outputColumnName: OnnxConfig.InputLayer,
                        outputAsFloatArray: true,
                        colorsToExtract: ImagePixelExtractingEstimator.ColorBits.Red
                    )); 


            var completePipeline = preProcessingPipeline.Append(onnxScorer);

            // Fit scoring pipeline to the ModelInput structure to create a model
            var emptyInput = _mlContext.Data.LoadFromEnumerable(new List<InputModel>());
            var model = completePipeline.Fit(emptyInput);

            return model;
        }
    }
}
