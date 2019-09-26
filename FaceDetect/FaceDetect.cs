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
using FaceDetection;
using System.Threading;

namespace VideoSurveillance
{
    public partial class FaceDetect : Form
    {
        [DllImport("msvcrt.dll")]
        public static extern int _putenv_s(string e, string v);

        private static VideoCapture _cameraCapture;

        private static IBackgroundSubtractor _fgDetector;
        private static Emgu.CV.Cvb.CvBlobDetector _blobDetector;
        private static Emgu.CV.Cvb.CvTracks _tracker;

        public FaceDetect()
        {
            InitializeComponent();
            Run();
        }

        void Run()
        {
            try
            {
                var trt = CvInvoke.BuildInformation;

                _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "rtsp_transport;udp");
                // _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "");
                _cameraCapture = new VideoCapture("rtsp://admin:Face1234@192.168.5.49:554/onvif1", VideoCapture.API.Ffmpeg);                
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

            _fgDetector = new BackgroundSubtractorMOG2();
            _blobDetector = new CvBlobDetector();
            _tracker = new CvTracks();

            Application.Idle += ProcessFrame;
        }

        static bool frameProcessing = false;
        static Mat LastProcessedFrame = new Mat();
        static Thread Twsdetecting;
        
        public static void detectAndDisplayFunc(Mat frame)
        {           
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            DetectFace.Detect(
           frame, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
           faces, eyes,
           out detectionTime);

            //paint faces
            foreach (Rectangle face in faces)
                CvInvoke.Rectangle(frame, face, new Bgr(Color.Red).MCvScalar, 2);

            //paint eyes
            foreach (Rectangle eye in eyes)
                CvInvoke.Rectangle(frame, eye, new Bgr(Color.Blue).MCvScalar, 2);
            LastProcessedFrame = frame;
            frameProcessing = false;
        }

        void ProcessFrame(object sender, EventArgs e)
        {
            if (!_cameraCapture.IsOpened)
                return;

            Mat frame = _cameraCapture.QueryFrame();

            if (frame == null)
                return;
            if (frameProcessing)
                return;

            imageBox1.Image = LastProcessedFrame;

            //skip frames due to poor performance       
            var twsDetectThread = new ThreadDetectFace(frame);           
            frameProcessing = true;
            Twsdetecting = new Thread(new ThreadStart(twsDetectThread.CallDetect));
            Twsdetecting.Start();

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
    }

    public class ThreadDetectFace
    {
        // State information used in the task.
        
        Mat _frame;

        // The constructor obtains the state information.
        public ThreadDetectFace(Mat frame)
        {
            _frame = frame;            
        }

        // The thread procedure performs the task, such as formatting
        // and printing a document.
        public void CallDetect()
        {
            FaceDetect.detectAndDisplayFunc(_frame);
        }

    }
}