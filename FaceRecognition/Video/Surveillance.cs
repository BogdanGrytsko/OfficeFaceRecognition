using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using CommonObjects;
using Emgu.CV;
using FaceRecognition.BL;
using FaceRecognition.Door;

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
        private readonly DoorManager door;
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
            door = new DoorManager();
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Success : {label}, Dist : {distance}");
            door.Open();
        }

        private void RecognitionFail(double distance, string label)
        {
            Console.ForegroundColor = ConsoleColor.Red;
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

        public void EnsureTrained()
        {
            if (File.Exists(trainedModel))
            {
                Console.WriteLine("[INFO] Model exists, loading");
                recognitionModule.Load(trainedModel);
            }
            else
            {
                Console.WriteLine("[INFO] Model doesn't exist, started training");
                Train();
            }
        }

        private void OnImageGrabbed(Mat mat)
        {
            var sw = new Stopwatch();
            sw.Start();
            ImageGrabbed?.Invoke(mat);
            //skip 2/3 of the frames, due to too much work on CPU
            //if (counter++ % 3 != 0) return;
            var (faces, eyes) = faceEyeDetector.Detect(mat);
            if (!faces.Any() || !eyes.Any()) return;
            PersonDetected?.Invoke(mat);
            FaceDetected?.Invoke(mat, faces, eyes);
            Console.WriteLine($"Frame processing time : {sw.Elapsed}");
        }
    }
}