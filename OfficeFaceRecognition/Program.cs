using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CommandLine;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;

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
            foreach (var imagePath in imagePaths)
            {
                var personName = Path.GetFileName(Path.GetDirectoryName(imagePath));
                Console.WriteLine($"Processing image {imagePath}");
                var image = CvInvoke.Imread(imagePath);
                //todo : here was the step with resizing to 600, investigate
                //image = imutils.resize(image, width = 600)
                //    (h, w) = image.shape[:2]
                var resizedThreeHundred = new Mat();
                CvInvoke.Resize(image, resizedThreeHundred, new Size(300, 300));
                var blobFromImage = DnnInvoke.BlobFromImage(
                    resizedThreeHundred, 1, new Size(300, 300), new MCvScalar(104.0, 177.0, 123.0));
                detector.SetInput(blobFromImage);
                var detection = detector.Forward();
                var data = (float[,,,]) detection.GetData();
                var maxi = 0;
                for (int i = 0; i < detection.SizeOfDimension[2]; i++)
                {
                    if (data[0, 0, i, 2] > maxi)
                        maxi = i;
                }

                var confidence = data[0, 0, maxi, 2];
                if (confidence > facePars.Confidence)
                {
                    Console.WriteLine($"Confident {confidence}, computing boxes");
                }
            }
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
