using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Face;
using FaceRecognition.BL;
using FaceRecognition.Storage;

namespace FaceRecognition.Video
{
    public class Surveillance
    {
        private const string oldCamera = "rtsp://admin:Face1234@192.168.5.49:554/onvif1", newCamera = "rtsp://192.168.5.5:8554/mjpeg/1";
        private int counter;

        private readonly IVideoGrab videoGrab;
        private readonly FaceEyeDetector faceEyeDetector;
        private readonly ITrainDataDAL trainDataDAL;
        private readonly FaceRecognitionModule recognitionModule;

        public event Action<Mat, List<Rectangle>, List<Rectangle>> FaceDetected;
        private event Action<Mat> PersonDetected;
        public event Action<Mat> ImageGrabbed;
        public event Action<FaceRecognizer.PredictionResult> PersonRecognized;

        public Surveillance(ITrainDataDAL trainDataDAL)
        {
            this.trainDataDAL = trainDataDAL;
            //videoGrab = new VideoGrab(newCamera);
            //videoGrab = new VideoGrab();
            videoGrab = new MockVideoGrab(trainDataDAL.GetImages().Take(100).ToList(), TimeSpan.FromMilliseconds(150));
            faceEyeDetector = new FaceEyeDetector("Models\\haarcascade_frontalface_default.xml", "Models\\haarcascade_eye.xml");
            recognitionModule = new FaceRecognitionModule();
            //recognitionModule.Load("Embeddings.trained");
            videoGrab.ImageGrabbed += OnImageGrabbed;
            PersonDetected += mat => DebugHelper.Save(mat.Bitmap);
            //PersonDetected += mat => PersonRecognized?.Invoke(recognitionModule.Predict(mat));
        }

        public void Start()
        {
            videoGrab.Start();
        }

        private void OnImageGrabbed(Mat mat)
        {
            ImageGrabbed?.Invoke(mat);
            //skip 2/3 of the frames, due to too much work on CPU
            //if (counter++ % 3 != 0) return;
            var (faces, eyes) = faceEyeDetector.Detect(mat);
            if (!faces.Any() || !eyes.Any()) return;
            PersonDetected?.Invoke(mat);
            FaceDetected?.Invoke(mat, faces, eyes);
        }
    }
}