using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using FaceRecognition;
using FaceRecognition.Storage;
using FaceRecognition.Video;

namespace VideoSurveillance
{
    public partial class FaceDetectForm : Form
    {
        private readonly Surveillance surveillance;

        public FaceDetectForm()
        {
            InitializeComponent();
            surveillance = new Surveillance(new FileSystemDAL(DebugHelper.OutputPath));
            Run();
        }

        private void Run()
        {
            surveillance.ImageGrabbed += SurveillanceOnImageGrabbed;
            surveillance.FaceDetected += SurveillanceOnFaceDetected;
            surveillance.PersonRecognized += SurveillanceOnPersonRecognized;
            surveillance.Start();
        }

        private void SurveillanceOnImageGrabbed(Mat mat)
        {
            imageBox1.Image = mat;
        }

        private void SurveillanceOnFaceDetected(Mat mat, List<Rectangle> faces, List<Rectangle> eyes)
        {
            //todo : maybe just put additional level of rectangles on what is already there;
            var frame = mat.Clone();
            foreach (Rectangle face in faces)
                CvInvoke.Rectangle(frame, face, new Bgr(Color.Red).MCvScalar, 2);

            foreach (Rectangle eye in eyes)
                CvInvoke.Rectangle(frame, eye, new Bgr(Color.Blue).MCvScalar, 2);

            imageBox1.Image = frame;
        }

        private void SurveillanceOnPersonRecognized(FaceRecognizer.PredictionResult prediction)
        {
            Console.WriteLine($"{prediction.Label}, {prediction.Distance}");
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (playButton.Checked)
                surveillance.ImageGrabbed += SurveillanceOnImageGrabbed;
            else
                surveillance.ImageGrabbed -= SurveillanceOnImageGrabbed;
        }

        private void detectionButton_Click(object sender, EventArgs e)
        {
            if (detectionButton.Checked)
                surveillance.FaceDetected += SurveillanceOnFaceDetected;
            else
                surveillance.FaceDetected -= SurveillanceOnFaceDetected;
        }
    }
}