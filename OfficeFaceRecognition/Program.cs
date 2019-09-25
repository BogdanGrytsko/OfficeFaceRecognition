using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommandLine;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using OfficeFaceRecognition.BL;

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
            var images = GetImages(facePars.DataSet).ToList();
            var detectionModule = new DetectionModule(facePars);
            var faces = detectionModule
                .GetFaces(images)
                .Take(12)
                .Select(f => (GetPersonName(f.Item1), f.Item2))
                .ToList();
            var labelMap = new LabelMap(faces.Select(f => f.Item1).Distinct());
            var labeledFaces = faces.Select(f => (labelMap.Map[f.Item1], f.Item2));

            var recognitionModule = new FaceRecognitionModule();
            recognitionModule.Train(labeledFaces.ToList(), facePars.Embeddings);
            foreach (var image in images)
            {
                var testImg = detectionModule.ProcessImage(image.Item2);
                var prediction = recognitionModule.Predict(testImg);
                Console.WriteLine($"Img name : {image.Item1} Prediction: {labelMap.ReverseMap[prediction.Label]}, Dist : {prediction.Distance}");
            }
        }

        private static IEnumerable<(int, Mat)> GetLabeledFaces(List<(string, Mat)> faces)
        {
            var idx = 1;
            var labelMap = faces.Select(f => f.Item1).Distinct().ToDictionary(f => f, f => idx++);
            return faces.Select(f => (labelMap[f.Item1], f.Item2));
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
