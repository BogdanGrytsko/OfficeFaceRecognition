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
            var detector = GetDetector(facePars);
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
                var (h, w) = (image.Size.Height, image.Size.Width);
                var resizedThreeHundred = new Mat();
                CvInvoke.Resize(image, resizedThreeHundred, new Size(300, 300));
                var blobFromImage = DnnInvoke.BlobFromImage(
                    resizedThreeHundred, 1, new Size(300, 300), new MCvScalar(104.0, 177.0, 123.0));
                detector.SetInput(blobFromImage);
                var detection = detector.Forward();
                var data = (float[,,,]) detection.GetData();
                //ensure at least one face was found
                if (data.Length == 0) continue;

                //we're making the assumption that each image has only ONE
                //face, so find the bounding box with the largest probability
                var maxi = GetMaxConfidenceIdx(detection, data);
                var confidence = data[0, 0, maxi, 2];
                if (confidence < facePars.Confidence) continue;
                //compute the (x, y)-coordinates of the bounding box for the face
                var (startX, startY, endX, endY) = (data[0, 0, maxi, 3] * w, data[0, 0, maxi, 4] * h,
                    data[0, 0, maxi, 5] * w, data[0, 0, maxi, 6] * h);
                
                //var face = image.
                //# extract the face ROI and grab the ROI dimensions
                //                face = image[startY: endY, startX: endX]
                //                    (fH, fW) = face.shape[:2]

                //# ensure the face width and height are sufficiently large
                //                if fW < 20 or fH< 20:
                //                continue
                Console.WriteLine($"Confident {confidence}, computing boxes");
            }
        }

        private static Net GetDetector(FaceRecognitionParams facePars)
        {
            Console.WriteLine("[INFO] loading face detector...");
            var protoPath = Path.Combine(facePars.Detector, "deploy.prototxt");
            var modelPath = Path.Combine(facePars.Detector, "res10_300x300_ssd_iter_140000.caffemodel");
            var detector = DnnInvoke.ReadNetFromCaffe(protoPath, modelPath);
            return detector;
        }

        private static int GetMaxConfidenceIdx(Mat detection, float[,,,] data)
        {
            var maxi = 0;
            var maxConfidence = 0f;
            for (int i = 0; i < detection.SizeOfDimension[2]; i++)
            {
                if (data[0, 0, i, 2] > maxConfidence)
                {
                    maxi = i;
                    maxConfidence = data[0, 0, i, 2];
                }
            }

            return maxi;
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
