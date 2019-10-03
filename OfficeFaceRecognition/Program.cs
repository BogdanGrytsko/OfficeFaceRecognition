using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using OfficeFaceRecognition.BL;
using OfficeFaceRecognition.Storage;

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
            var images = new FileSystemDAL(facePars.DataSet).GetImages().ToList();
            var detectionModule = new DetectionModule(facePars);

            var faces = detectionModule
                .GetFaces(images)
                .Select(f => (GetPersonName(f.Item1), f.Item2))
                .ToList();
            var labelMap = new LabelMap(faces.Select(f => f.Item1).Distinct());
            var labeledFaces = faces.Select(f => (labelMap.Map[f.Item1], f.Item2));

            var testImages = new FileSystemDAL(facePars.TestSet).GetImages().ToList();
            var recognitionModule = new FaceRecognitionModule();
            recognitionModule.Train(labeledFaces.ToList(), facePars.Embeddings);

            foreach (var image in testImages)
            {
                var testImg = detectionModule.ProcessImage(image.Image);
                var prediction = recognitionModule.Predict(testImg);
                Console.WriteLine($"Img name : {image.Label} Prediction: {labelMap.ReverseMap[prediction.Label]}, Dist : {prediction.Distance}");
            }
        }

        private static IEnumerable<(string, byte[])> GetImages(string dataSet)
        {
            var imagePaths = Directory.GetFiles(dataSet, "*.*", SearchOption.AllDirectories);
            foreach (var imagePath in imagePaths)
            {
                yield return (imagePath, File.ReadAllBytes(imagePath));
            }
        }

        private static string GetPersonName(string imagePath)
        {
            return Path.GetFileName(Path.GetDirectoryName(imagePath));
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
