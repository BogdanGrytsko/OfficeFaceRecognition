using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;

namespace FaceRecognition.BL
{
    public class CudaFaceEyeDetector
    {
        private readonly CudaCascadeClassifier faceClassifier, eyeClassifier;

        public CudaFaceEyeDetector(string faceFileName, string eyeFileName)
        {
            System.Console.WriteLine($"Have I Cuda? {CudaInvoke.HasCuda}");
            faceClassifier = new CudaCascadeClassifier(faceFileName)
            {
                ScaleFactor = 1.1,
                MinNeighbors = 10,
                MinObjectSize = Size.Empty
            };
            eyeClassifier = new CudaCascadeClassifier(eyeFileName)
            {
                ScaleFactor = 1.1,
                MinNeighbors = 10,
                MinObjectSize = new Size(20, 20)
            };
        }

        public (List<Rectangle>, List<Rectangle>) Detect(IInputArray image)
        {
            var faces = new List<Rectangle>();
            var eyes = new List<Rectangle>();
            using (var ugray = new GpuMat())
            {
                CudaInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                //normalizes brightness and increases contrast of the image
                CudaInvoke.EqualizeHist(ugray, ugray);

                //Detect the faces  from the gray scale image and store the locations as rectangle
                //The first dimensional is the channel
                //The second dimension is the index of the rectangle in the specific channel

                var facesDetected = GetRectangles(faceClassifier, ugray);
                faces.AddRange(facesDetected);

                foreach (var f in facesDetected)
                {
                    //Get the region of interest on the faces
                    using (var faceRegion = new GpuMat(ugray, new Range(f.Bottom, f.Top), new Range(f.Left, f.Right)))
                    {
                        var eyesDetected = GetRectangles(eyeClassifier, faceRegion);

                        foreach (var e in eyesDetected)
                        {
                            var eyeRect = e;
                            eyeRect.Offset(f.X, f.Y);
                            eyes.Add(eyeRect);
                        }
                    }
                }
            }

            return (faces, eyes);
        }

        private static Rectangle[] GetRectangles(CudaCascadeClassifier classifier, GpuMat region)
        {
            var facesBufGpu = new GpuMat();
            classifier.DetectMultiScale(region, facesBufGpu);
            return classifier.Convert(facesBufGpu);
        }
    }
}
