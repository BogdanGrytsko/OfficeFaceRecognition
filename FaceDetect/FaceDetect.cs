//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FaceDetect.Detection;
using OfficeFaceRecognition.Video;
using System.Diagnostics;

namespace VideoSurveillance
{
    public partial class FaceDetect : Form
    {
        [DllImport("msvcrt.dll")]
        public static extern int _putenv_s(string e, string v);

        private static VideoCapture cameraCapture;

        private static IBackgroundSubtractor fgDetector;
        private static CvBlobDetector blobDetector;
        private static CvTracks tracker;
        private readonly FaceEyeDetector detector;

        public FaceDetect()
        {
            InitializeComponent();
            detector = new FaceEyeDetector("haarcascade_frontalface_default.xml", "haarcascade_eye.xml");
            Run();
        }

        void Run()
        {
            try
            {
                var trt = CvInvoke.BuildInformation;

                _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "rtsp_transport;udp");
                // _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "");
                cameraCapture = new VideoCapture("rtsp://admin:Face1234@192.168.5.49:554/onvif1", VideoCapture.API.Ffmpeg);
                //_cameraCapture = new VideoCapture("http://192.168.1.90:81/stream?x.mjpeg", VideoCapture.API.Any);
                //_cameraCapture = new VideoCapture("rtsp://192.168.1.90:8554", VideoCapture.API.Ffmpeg);
                //_cameraCapture = new VideoCapture("http://192.168.1.90/?x.mjpeg", VideoCapture.API.Ffmpeg);
                //_cameraCapture = new VideoCapture("http://192.168.1.90", VideoCapture.API.Any);

                // Mat _frame = new Mat();
                // Mat _frameCopy = new Mat();
                // _cameraCapture.Read(_frame);
                //// _cameraCapture.Retrieve(_frame, 0);
                // _frame.CopyTo(_frameCopy);              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            fgDetector = new BackgroundSubtractorMOG2();
            blobDetector = new CvBlobDetector();
            tracker = new CvTracks();

            Application.Idle += ProcessFrame;
        }

        bool frameProcessing = false;
        bool play = true;
        bool detection = true;

        public Mat DetectAndDisplay(Mat frame)
        {
            var (faces, eyes) = detector.Detect(frame);

            foreach (Rectangle face in faces)
                CvInvoke.Rectangle(frame, face, new Bgr(Color.Red).MCvScalar, 2);

            foreach (Rectangle eye in eyes)
                CvInvoke.Rectangle(frame, eye, new Bgr(Color.Blue).MCvScalar, 2);

            return frame;
        }

        async void ProcessFrame(object sender, EventArgs e)
        {
            if (!cameraCapture.IsOpened || !play)
                return;

            Mat frame = cameraCapture.QueryFrame();

            if (frame == null)
                return;

            if (!detection)
            {
                imageBox1.Image = frame;
                return;
            }

            if (frameProcessing)
                return;

            frameProcessing = true;

            imageBox1.Image = await Task.Run(() =>
            {
                return DetectAndDisplay(frame);
            });

            //skip frames due to poor performance       
            frameProcessing = false;

            //filter out noises
            //  Mat smoothedFrame = new Mat();
            // CvInvoke.GaussianBlur(frame, smoothedFrame, new Size(3, 3), 1); 
            //frame._SmoothGaussian(3);                 

            //resize if needed
            //            using (Mat smallFrame = new Mat())
            //{
            //  Mat smallFrame = new Mat();
            // Size smallSize = new Size(720, 576);
            //  CvInvoke.Resize(frame, smallFrame, smallSize, 0, 0, Inter.Cubic);

            //  imageBox1.Image = frame;
            //imageBox2.Image = smallFrame;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            play = playButton.Checked;
        }

        private void reprocessButton_Click(object sender, EventArgs e)
        {
            imageBox1.Image = DetectAndDisplay(imageBox1.Image as Mat);
        }

        private void detectionButton_Click(object sender, EventArgs e)
        {
            detection = detectionButton.Checked;
        }
    }
}