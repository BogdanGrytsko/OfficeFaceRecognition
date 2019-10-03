using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Face;

namespace FaceRecognition.BL
{
    public class FaceRecognitionModule
    {
        private readonly FaceRecognizer recognizer;

        public FaceRecognitionModule()
        {
            recognizer = new EigenFaceRecognizer();
        }

        public void Load(string path)
        {
            recognizer.Read(path);
        }

        public void Train(IList<(int, Mat)> labeledFaces, string path)
        {
            recognizer.Train(labeledFaces.Select(f => f.Item2).ToArray(), labeledFaces.Select(f => f.Item1).ToArray());
            recognizer.Write(path);
        }

        public FaceRecognizer.PredictionResult Predict(Mat image)
        {
            return recognizer.Predict(image);
        }
    }
}