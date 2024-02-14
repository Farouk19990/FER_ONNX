using CLI;
using System;
using System.IO;
using System.Linq;

namespace FacialExpressionDetectorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Image facial expression detector - using FER+ ONNX model");
            Console.WriteLine();

            // Load all image paths
            var imageFolder = GetImageFolderFromArgs(args);
            var imagePaths = Directory.GetFiles(imageFolder,
                                                "*.*")
                                      .Select(Path.GetFullPath)
                                      .ToArray();

            foreach(var i in imagePaths)
            {
                Console.WriteLine($"yoo i : {i}");
            }
            // Run the images through the ONNX model
            var facialExpressionDetector = new FacialExpressionDetector();

            Console.WriteLine("Scoring all images...");
            var timeStamp = DateTime.Now;

            var emotionProbabilities = facialExpressionDetector
                                        .DetectEmotionsInImageFiles(imagePaths);

            Console.WriteLine($"Images scored in {(DateTime.Now - timeStamp).TotalSeconds} seconds");
            Console.WriteLine();


            // Print the results
            foreach (var fileWithScore in emotionProbabilities)
            {
                Console.Write("* ");

                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine($"{fileWithScore.Filename}");

                Console.ForegroundColor = ConsoleColor.Gray;

                foreach (var emotion in fileWithScore.EmotionProbabilities)
                {
                    if (emotion.probability > 0.1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.Write($"[{emotion.probability:P0}] {emotion.emotion}  ");

                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
                Console.WriteLine();

            }

        }

        private static string GetImageFolderFromArgs(string[] args)
        {
            //Default to "images" folder if no arguments are given
            if (args.Length != 1) return "images";

            if (Directory.Exists(args[0])) return args[0];

            Console.WriteLine("Given image directory does not exist");
            Environment.Exit(1);

            return null;

        }
    }
}
