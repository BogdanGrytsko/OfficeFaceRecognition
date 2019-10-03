using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using FaceRecognition.Storage;

namespace FaceRecognition.BL
{
    public class DetectionModule
    {
        private readonly Net detector, embedder;
        private readonly double minConfidence;

        public DetectionModule(string embeddingModel, double minConfidence)
        {
            Console.WriteLine("[INFO] loading face detector...");
            detector = GetDetector();
            Console.WriteLine("[INFO] load serialized face embedding model from disk...");
            embedder = DnnInvoke.ReadNet(embeddingModel);
            this.minConfidence = minConfidence;
        }

        public DetectionModule(byte[] proto, byte[] caffeModel, string embeddingModel, double minConfidence)
        {
            detector = DnnInvoke.ReadNetFromCaffe(proto, caffeModel);
            //todo : is there a way to read embeddingModel from bytes? it's a torch model.
            embedder = DnnInvoke.ReadNet(embeddingModel);
            this.minConfidence = minConfidence;
        }

        public IEnumerable<(string, Mat)> GetFaces(IEnumerable<ImageLabel> images)
        {
            Console.WriteLine("[INFO] quantifying faces...");
            foreach (var image in images)
            {
                Console.WriteLine($"Processing image {image.Label}");

                var res = ProcessImage(image.Image);
                if (res != null)
                    yield return (image.Label, res);
            }
        }

        public Mat ProcessImage(byte[] byteImage)
        {
            var image = new Mat();
            CvInvoke.Imdecode(byteImage, ImreadModes.AnyColor, image);
            //#load the image, resize it to have a width of 600 pixels (while
            //# maintaining the aspect ratio), and then grab the image dimension

            var (h, w) = (image.Size.Height, image.Size.Width);
            var resizedThreeHundred = new Mat();
            var image600 = new Mat();
            CvInvoke.Resize(image, image600, new Size(600, 600 * image.Height / image.Width));
            CvInvoke.Resize(image600, resizedThreeHundred, new Size(300, 300));
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
            if (confidence < minConfidence) return null;
            //compute the (x, y)-coordinates of the bounding box for the face
            var (startX, startY, endX, endY) = (data[0, 0, maxi, 3] * w, data[0, 0, maxi, 4] * h,
                data[0, 0, maxi, 5] * w, data[0, 0, maxi, 6] * h);

            //extract the face ROI and grab the ROI dimensions
            var faceRect = Rectangle.FromLTRB((int)startX, (int)startY, (int)endX, (int)endY);

#if DEBUG
            //DebugHelper.DrawFaceRectAndSave(image.Bitmap, faceRect);
#endif

            var face = new Mat(image, faceRect);
            var (fH, fW) = (face.Size.Height, face.Size.Width);
            if (fH < 20 || fW < 20) return null;

            //construct a blob for the face ROI, then pass the blob
            //through our face embedding model to obtain the 128-d
            //quantification of the face
            var faceBlob = DnnInvoke.BlobFromImage(face, 1.0 / 255, new Size(96, 96), new MCvScalar(0, 0, 0),
                swapRB: true, crop: false);
            embedder.SetInput(faceBlob);
            var vec = embedder.Forward().Clone();
            return vec.Reshape(1);
        }

        private static Net GetDetector()
        {
            var proto = Utils.GetResourceBytes("Models.deploy.prototxt");
            var model = Utils.GetResourceBytes("Models.res10_300x300_ssd_iter_140000.caffemodel");
            return DnnInvoke.ReadNetFromCaffe(proto, model);
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