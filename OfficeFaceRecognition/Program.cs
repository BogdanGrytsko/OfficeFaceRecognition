using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Emgu.CV.Dnn;

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
            Console.WriteLine("[INFO] loading face detector...");
            var protoPath = Path.Combine(facePars.Detector, "deploy.prototxt");
            var modelPath = Path.Combine(facePars.Detector, "res10_300x300_ssd_iter_140000.caffemodel");
            var detector = DnnInvoke.ReadNetFromCaffe(protoPath, modelPath);
            Console.WriteLine("[INFO] load serialized face embedding model from disk...");
            var embedder = DnnInvoke.ReadNet(facePars.EmbeddingModel);
            Console.WriteLine("[INFO] quantifying faces...");
            var imagePaths = Directory.GetFiles(facePars.DataSet, "*.*", SearchOption.AllDirectories);
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
