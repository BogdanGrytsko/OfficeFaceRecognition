using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonObjects;
using Emgu.CV;
using Emgu.CV.Face;
using FaceRecognition.BL;

namespace FaceRecognition.Video
{
    public class Surveillance
    {
        private const string oldCamera = "rtsp://admin:Face1234@192.168.5.49:554/onvif1", newCamera = "rtsp://192.168.5.5:8554/mjpeg/1";
        private const string trainedModel = "Embeddings.trained", faceEmbeddingsModel = "Models\\openface_nn4.small2.v1.t7";
        private int counter;

        private readonly IVideoGrab videoGrab;
        private readonly ITrainDataDAL trainDataDAL;
        private readonly FaceRecognitionModule recognitionModule;
        private readonly FaceEyeDetector faceEyeDetector;
        private readonly DetectionModule detectionModule;
        private readonly double confidence;
        private LabelMap labelMap;

        public event Action<Mat, List<Rectangle>, List<Rectangle>> FaceDetected;
        private event Action<Mat> PersonDetected;
        public event Action<Mat> ImageGrabbed;
        public event Action<FaceRecognizer.PredictionResult> PersonRecognized;

        public Surveillance(ITrainDataDAL trainDataDAL, double confidence = 0.5)
        {
            this.trainDataDAL = trainDataDAL;
            this.confidence = confidence;
            //videoGrab = new VideoGrab(newCamera);
            //videoGrab = new VideoGrab();
            videoGrab = new MockVideoGrab(trainDataDAL.GetImages().Take(100).ToList(), TimeSpan.FromMilliseconds(150));
            faceEyeDetector = new FaceEyeDetector("Models\\haarcascade_frontalface_default.xml", "Models\\haarcascade_eye.xml");
            recognitionModule = new FaceRecognitionModule();
            detectionModule = new DetectionModule(faceEmbeddingsModel, confidence);
            recognitionModule.Load(trainedModel);
            labelMap = new LabelMap(trainDataDAL.GetLabelMap());
            videoGrab.ImageGrabbed += OnImageGrabbed;
            PersonDetected += OnPersonDetected;
        }

        private void OnPersonDetected(Mat mat)
        {
            var prediction = GetPrediction(mat);
            if (prediction.Distance >= confidence)
                RecognitionFail(prediction);
            else
                RecognitionSuccess(prediction);
        }

        public (double, string) Predict(Mat mat)
        {
            var prediction = GetPrediction(mat);
            return (prediction.Distance, labelMap.ReverseMap[prediction.Label]);
        }

        private FaceRecognizer.PredictionResult GetPrediction(Mat mat)
        {
            var faceEmb = detectionModule.GetFaceEmbedding(mat);
            return recognitionModule.Predict(faceEmb);
        }

        private void RecognitionSuccess(FaceRecognizer.PredictionResult prediction)
        {
            Console.WriteLine($"Success : {labelMap.ReverseMap[prediction.Label]}, Dist : {prediction.Distance}");
        }

        private void RecognitionFail(FaceRecognizer.PredictionResult prediction)
        {
            Console.WriteLine($"Failure : {labelMap.ReverseMap[prediction.Label]}, Dist : {prediction.Distance}");
        }

        public void Start()
        {
            videoGrab.Start();
        }

        public void Train()
        {
            videoGrab.Pause();
            var images = trainDataDAL.GetImages().ToList();
            labelMap = new LabelMap(trainDataDAL.GetLabelMap());
            var faceEmbeddings = images
                .Select(img => (labelMap.Map[img.Label], detectionModule.GetFaceEmbedding(img.Image)))
                .ToList();
            recognitionModule.Train(faceEmbeddings, trainedModel);
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