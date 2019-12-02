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

        public void Train(IList<(int, Mat)> labeledFaces, string path, string personalModelPrefix)
        {
            recognizer.Train(labeledFaces.Select(f => f.Item2).ToArray(), labeledFaces.Select(f => f.Item1).ToArray());
            recognizer.Write(path);
            foreach (var label in labeledFaces.GroupBy(e => e.Item1))
            {
                using (var recognizerPerson = new EigenFaceRecognizer())
                {
                    recognizerPerson.Train(label.Select(b=>b.Item2).ToArray(), label.Select(a => label.Key).ToArray());
                    recognizerPerson.Write(string.Format("{0}{1}.trained", personalModelPrefix, label.Key.ToString()));
                }
            }
        }

        public FaceRecognizer.PredictionResult Predict(Mat image)
        {
            return recognizer.Predict(image);
        }
    }
}