using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Face;
using OfficeFaceRecognition.BL;
using OfficeFaceRecognition.Storage;

namespace OfficeFaceRecognition.Video
{
    public class Surveillance
    {
        private int counter;

        private readonly VideoGrab videoGrab;
        private readonly FaceEyeDetector faceEyeDetector;
        private readonly FaceDAL dal;
        private readonly FaceRecognitionModule recognitionModule;

        public event Action<Mat, List<Rectangle>, List<Rectangle>> FaceDetected;
        private event Action<Mat> PersonDetected;
        public event Action<Mat> ImageGrabbed;
        public event Action<FaceRecognizer.PredictionResult> PersonRecognized;

        public Surveillance()
        {
            videoGrab = new VideoGrab("rtsp://admin:Face1234@192.168.5.49:554/onvif1");
            faceEyeDetector = new FaceEyeDetector("haarcascade_frontalface_default.xml", "haarcascade_eye.xml");
            dal = new FaceDAL();
            recognitionModule = new FaceRecognitionModule();
            //recognitionModule.Load("Embeddings.trained");
            videoGrab.ImageGrabbed += OnImageGrabbed;
            PersonDetected += mat => dal.Save(mat);
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
            if (counter++ % 3 != 0) return;
            var (faces, eyes) = faceEyeDetector.Detect(mat);
            if (!faces.Any() || !eyes.Any()) return;
            PersonDetected?.Invoke(mat);
            FaceDetected?.Invoke(mat, faces, eyes);
        }
    }
}