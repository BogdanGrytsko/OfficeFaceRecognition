using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
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
            var surveillance = new Surveillance(VideoGrabFactory.GetMockCamera(), new FileSystemDAL(facePars.DataSet), facePars.Confidence);
            surveillance.EnsureTrained();

            var testImages = new FileSystemDAL(facePars.TestSet).GetImages().ToList();

            foreach (var image in testImages)
            {
                var (distance, label) = surveillance.Predict(Utils.GetMat(image.Image));
                Console.WriteLine($"Img name : {image.Label} Prediction: {label}, Dist : {distance}");
            }
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
