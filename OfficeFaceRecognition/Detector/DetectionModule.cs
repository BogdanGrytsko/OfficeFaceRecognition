using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;

namespace OfficeFaceRecognition.Detector
{
    public class DetectionModule
    {
        private readonly FaceRecognitionParams facePars;
        private readonly Net detector, embedder;

        public DetectionModule(FaceRecognitionParams facePars)
        {
            this.facePars = facePars;
            detector = GetDetector(facePars);
            Console.WriteLine("[INFO] load serialized face embedding model from disk...");
            embedder = DnnInvoke.ReadNet(facePars.EmbeddingModel);
        }

        public List<(string, Mat)> GetLabeledFaces()
        {
            var list = new List<(string, Mat)>();
            Console.WriteLine("[INFO] quantifying faces...");
            var imagePaths = Directory.GetFiles(facePars.DataSet, "*.*", SearchOption.AllDirectories);
            foreach (var imagePath in imagePaths)
            {
                var res = ProcessImage(imagePath);
                if (res.HasValue)
                    list.Add(res.Value);
            }
            return list;
        }

        private (string, Mat)? ProcessImage(string imagePath)
        {
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
            if (data.Length == 0) return null;

            //we're making the assumption that each image has only ONE
            //face, so find the bounding box with the largest probability
            var maxi = GetMaxConfidenceIdx(detection, data);
            var confidence = data[0, 0, maxi, 2];
            if (confidence < facePars.Confidence) return null;
            //compute the (x, y)-coordinates of the bounding box for the face
            var (startX, startY, endX, endY) = (data[0, 0, maxi, 3] * w, data[0, 0, maxi, 4] * h,
                data[0, 0, maxi, 5] * w, data[0, 0, maxi, 6] * h);

            //extract the face ROI and grab the ROI dimensions
            var face = new Mat(image,
                new Rectangle((int) startX, (int) startY, (int) (endX - startX), (int) (endY - startY)));
            var (fH, fW) = (face.Size.Height, face.Size.Width);
            if (fH < 20 || fW < 20) return null;

            //construct a blob for the face ROI, then pass the blob
            //through our face embedding model to obtain the 128-d
            //quantification of the face
            var faceBlob = DnnInvoke.BlobFromImage(face, 1.0 / 255, new Size(96, 96), new MCvScalar(0, 0, 0),
                swapRB: true, crop: false);
            embedder.SetInput(faceBlob);
            var vec = embedder.Forward();
            var personName = Path.GetFileName(Path.GetDirectoryName(imagePath));
            return (personName, vec.Reshape(1));
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
    }
}