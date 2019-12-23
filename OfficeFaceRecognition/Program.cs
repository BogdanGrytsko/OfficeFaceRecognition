using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using FaceRecognition;
using FaceRecognition.BL;
using FaceRecognition.Storage;
using FaceRecognition.Video;

namespace OfficeFaceRecognition
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<FaceRecognitionParams>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        private static void RunOptions(FaceRecognitionParams facePars)
        {
            //var c = new CudaFaceEyeDetector("a", "b");
            var surveillance = new Surveillance(
                VideoGrabFactory.GetMockCamera(facePars.TestSet), new FileSystemDAL(facePars.DataSet), facePars.Confidence);
            surveillance.EnsureTrained();
            surveillance.Start();
            //RunTestData(surveillance, DebugHelper.OutputPath);
            Console.ReadKey();
        }

        private static void RunTestData(Surveillance surveillance, string testSet)
        {
            var testImages = new FileSystemDAL(testSet).GetImages().ToList();

            foreach (var image in testImages)
            {
                var (distance, label, labelId) = surveillance.Predict(Utils.GetMat(image.Image));
                Console.WriteLine($"Img name : {image.Label} Prediction: {label}, Dist : {distance}");
            }
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
