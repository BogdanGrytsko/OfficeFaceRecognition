using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;

namespace OfficeFaceRecognition.Video
{
    public class FaceEyeDetector
    {
        private readonly CascadeClassifier faceClassifier, eyeClassifier;

        public FaceEyeDetector(string faceFileName, string eyeFileName)
        {
            faceClassifier = new CascadeClassifier(faceFileName);
            eyeClassifier = new CascadeClassifier(eyeFileName);
        }

        public (List<Rectangle>, List<Rectangle>) Detect(Mat image)
        {
            var faces = new List<Rectangle>();
            var eyes = new List<Rectangle>();
            using (var ugray = new UMat())
            {
                CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                //normalizes brightness and increases contrast of the image
                CvInvoke.EqualizeHist(ugray, ugray);

                //Detect the faces  from the gray scale image and store the locations as rectangle
                //The first dimensional is the channel
                //The second dimension is the index of the rectangle in the specific channel                     
                var facesDetected = faceClassifier.DetectMultiScale(
                    ugray,
                    1.1,
                    10,
                    Size.Empty); // new Size(20, 20));

                faces.AddRange(facesDetected);

                foreach (var f in facesDetected)
                {
                    //Get the region of interest on the faces
                    using (var faceRegion = new UMat(ugray, f))
                    {
                        var eyesDetected = eyeClassifier.DetectMultiScale(
                            faceRegion,
                            1.1,
                            10,
                            new Size(20, 20));

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
    }
}