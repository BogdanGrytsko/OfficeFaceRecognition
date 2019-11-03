using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using FaceRecognition;
using FaceRecognition.Storage;
using FaceRecognition.Video;
using FaceRecognitionDatabase;

namespace VideoSurveillance
{
    public partial class FaceDetectForm : Form
    {
        private readonly Surveillance surveillance;

        private string LabelText
        {
            get
            {
                string text = string.Empty;
                textBoxLabel.Invoke(new MethodInvoker(delegate
                {
                    text = textBoxLabel.Text;
                }));
                return text;
            }
        }

        private string DetecctedPersonText
        {
            get
            {
                string text = string.Empty;
                labelDetectedPerson.Invoke(new MethodInvoker(delegate
                {
                    text = labelDetectedPerson.Text;
                }));
                return text;
            }
            set
            {
                string text = value;
                labelDetectedPerson.BeginInvoke(new MethodInvoker(delegate
                {
                    labelDetectedPerson.Text = text;
                }));
            }
        }

        public FaceDetectForm()
        {
            InitializeComponent();
            surveillance = new Surveillance(VideoGrabFactory.GetSelfCamera(), new FileSystemDAL(DebugHelper.OutputPath));
            Run();
        }

        private void Run()
        {
            surveillance.ImageGrabbed += SurveillanceOnImageGrabbed;
            surveillance.FaceDetected += SurveillanceOnFaceDetected;
            surveillance.RecognitionSuccessfull += Surveillance_RecognitionSuccessfull;
            surveillance.Start();
        }

        private void Surveillance_RecognitionSuccessfull(double distance, string label)
        {
            DetecctedPersonText = string.Format("{0} Label detected:{1}, distance:{2}", DateTime.Now, label, distance);
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

            imageBox2.Image = frame;
            var text = LabelText;
            if (checkBoxCapture.Checked && faces.Count == 1 && eyes.Count == 2 && text != null && text.Length > 0)
            {
                MemoryStream imageMemoryStream = new MemoryStream();
                Image imageToTrain = mat.Bitmap;
                imageToTrain.Save(imageMemoryStream, ImageFormat.Png);
                var directory = String.Format("{0}/Output", Directory.GetCurrentDirectory());
                var imageLabel = new ImageLabel(text, imageMemoryStream.ToArray());
                var dal = new FileSystemDAL(directory);
                dal.Add(imageLabel);
            }
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

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            if (!checkBoxCapture.Checked)
            {
                checkBoxEnableReccognition.Checked = false;
                surveillance.Train();
            }
        }

        private void checkBoxEnableReccognition_CheckedChanged(object sender, EventArgs e)
        {
            if (surveillance != null)
            {
                surveillance.RecognitionEnable = ((CheckBox)sender).Checked;
            }
        }
    }
}