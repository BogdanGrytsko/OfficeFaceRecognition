using System;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;

namespace FaceRecognition.BL
{
    public class DetectionModule
    {
        private readonly Net detector, embedder, embedderSecond;
        private readonly double minConfidence;
        FaceEyeDetector faceEyeDetector;

        public DetectionModule(string embeddingModel, string embeddingSecondModel, double minConfidence)
        {
            Console.WriteLine("[INFO] loading face detector...");
            detector = GetDetector();
            faceEyeDetector = new FaceEyeDetector("Models\\haarcascade_frontalface_default.xml", "Models\\haarcascade_eye.xml");            
            Console.WriteLine("[INFO] load serialized face embedding model from disk...");
            embedder = DnnInvoke.ReadNet(embeddingModel);
            embedderSecond = GetEmbedderFromCaffe("Models.deploy.prototxt", "Models.res10_300x300_ssd_iter_140000.caffemodel");
            this.minConfidence = minConfidence;
        }

        public DetectionModule(byte[] proto, byte[] caffeModel, string embeddingModel, string embeddingSecondModel, double minConfidence)
        {           
            //todo : is there a way to read embeddingModel from bytes? it's a torch model.
            embedder = DnnInvoke.ReadNet(embeddingModel);
            embedderSecond = GetEmbedderFromCaffe("Models.deploy.prototxt", "Models.res10_300x300_ssd_iter_140000.caffemodel");
            this.minConfidence = minConfidence;
        }

        public Mat GetFaceEmbedding(byte[] byteImage)
        {
            var image = Utils.GetMat(byteImage);
            return GetFaceEmbedding(image);
        }
        public Mat GetFaceEmbeddingSecond(byte[] byteImage)
        {
            var image = Utils.GetMat(byteImage);
            return GetFaceEmbeddingSecond(image);
        }

        /// <summary>
        /// detect face and crop, otherwise returns null, option1 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Mat GetDetectedFaceBox(Mat image)
        {
            var (faces, eyes) = faceEyeDetector.Detect(image);
            if (faces == null || faces.Count != 1)
            {
                return null;
            }

            return new Mat(image, faces[0]);
        }

        /// <summary>
        /// detect face and crop, otherwise returns null, option2
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Mat GetDetectedFaceBoxSecond(Mat image)
        {
            var (h, w) = (image.Size.Height, image.Size.Width);
            var resizedThreeHundred = new Mat();
            var image600 = new Mat();
            CvInvoke.Resize(image, image600, new Size(600, 600 * image.Height / image.Width));
            CvInvoke.ResizeForFrame(image600, resizedThreeHundred, new Size(300, 300));
            var blobFromImage = DnnInvoke.BlobFromImage(
                resizedThreeHundred, 1, new Size(300, 300), new MCvScalar(104.0, 177.0, 123.0));
            detector.SetInput(blobFromImage);
            var detection = detector.Forward();
            var data = (float[,,,])detection.GetData();
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
            // correct coordinates
            if (startX < 0) startX = 0;
            if (endX > image.Width) endX = image.Width;

            if (startY < 0) startY = 0;
            if (endY > image.Height) endY = image.Height;

            //extract the face ROI and grab the ROI dimensions
            var faceRect = Rectangle.FromLTRB((int)startX, (int)startY, (int)endX, (int)endY);
            return new Mat(resizedThreeHundred, faceRect);
        }

        public Mat GetFaceEmbedding(Mat image, int detectAndCropMethod = 1) // image should be a cropped face image
        {
            Mat face;
            switch (detectAndCropMethod)
            {
                case 1:                    
                        face = GetDetectedFaceBox(image);
                        break;
                case 2:
                    face = GetDetectedFaceBoxSecond(image);
                    break;
                default:
                    face = image; // the image has already detected and cropped
                    break;
            }
            if (face == null) 
                return null;

            //#load the image, resize it to have a width of 300 pixels (while
            //# maintaining the aspect ratio), and then grab the image dimension

#if DEBUG
            //DebugHelper.DrawFaceRectAndSave(image.Bitmap, faceRect);
#endif           

            var face96 = new Mat();
            CvInvoke.ResizeForFrame(face, face96, new Size(96, 96));

            var (fH, fW) = (face.Size.Height, face.Size.Width);
            if (fH < 96 || fW < 96) return null;

            //construct a blob for the face ROI, then pass the blob
            //through our face embedding model to obtain the 128-d
            //quantification of the face
            var faceBlob = DnnInvoke.BlobFromImage(face96, 1.0 / 255, new Size(96, 96), new MCvScalar(0, 0, 0),
                swapRB: true, crop: false);
            embedder.SetInput(faceBlob);
            var vec = embedder.Forward().Clone();
            return vec.Reshape(1);
        }

        public Mat GetFaceEmbeddingSecond(Mat image)
        {                      
            var (fH, fW) = (image.Size.Height, image.Size.Width);
            if (fH < 300 || fW < 300) 
                return null;

            var face300 = new Mat();
            CvInvoke.ResizeForFrame(image, face300, new Size(300, 300));
            var faceBlob = DnnInvoke.BlobFromImage(face300, 1.0 / 255 , new Size(300, 300), new MCvScalar(0, 0, 0),
                swapRB: true, crop: false);
            embedderSecond.SetInput(faceBlob);
            var vec = embedderSecond.Forward().Clone();
            return vec.Reshape(1);
        }

        private static Net GetEmbedderFromCaffe(string protoFileName, string modelFileName)
        {
            var proto = Utils.GetResourceBytes(protoFileName);
            var model = Utils.GetResourceBytes(modelFileName);
            return DnnInvoke.ReadNetFromCaffe(proto, model);
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