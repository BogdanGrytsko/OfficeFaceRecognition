using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommandLine;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using OfficeFaceRecognition.Detector;

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
            var detectionModule = new DetectionModule(facePars);
            var faces = detectionModule
                .GetFaces(GetImages(facePars.DataSet))
                .Select(f => (GetPersonName(f.Item1), f.Item2))
                .ToList();
            
            Console.WriteLine(faces.Count);
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
