using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonObjects;
using Emgu.CV;
using FaceRecognition.BL;

namespace FaceRecognition.Video
{
    public class Surveillance
    {
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

        public Surveillance(IVideoGrab videoGrab, ITrainDataDAL trainDataDAL, double confidence = 0.5)
        {
            this.videoGrab = videoGrab;
            this.trainDataDAL = trainDataDAL;
            this.confidence = confidence;
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
            var (distance, label) = Predict(mat);
            if (distance >= confidence)
                RecognitionFail(distance, label);
            else
                RecognitionSuccess(distance, label);
        }

        public (double, string) Predict(Mat mat)
        {
            var faceEmb = detectionModule.GetFaceEmbedding(mat);
            if (faceEmb == null)
                return (1, "Couldn't extract face embedding");
            var prediction = recognitionModule.Predict(faceEmb);
            return (prediction.Distance, labelMap.ReverseMap[prediction.Label]);
        }

        private void RecognitionSuccess(double distance, string label)
        {
            Console.WriteLine($"Success : {label}, Dist : {distance}");
        }

        private void RecognitionFail(double distance, string label)
        {
            Console.WriteLine($"Failure : {label}, Dist : {distance}");
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
                .Where(tuple => tuple.Item2 != null)
                .ToList();
            recognitionModule.Train(faceEmbeddings, trainedModel);
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