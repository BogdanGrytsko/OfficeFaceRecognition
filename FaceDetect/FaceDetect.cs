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
using FaceDetect.Detection;

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

        static bool frameProcessing = false;
        static Mat lastProcessedFrame = new Mat();
        static Thread twsdetecting;

        public static void DetectAndDisplayFunc(Mat frame)
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
            lastProcessedFrame = frame;
            frameProcessing = false;
        }

        void ProcessFrame(object sender, EventArgs e)
        {
            if (!cameraCapture.IsOpened)
                return;

            Mat frame = cameraCapture.QueryFrame();

            if (frame == null)
                return;
            if (frameProcessing)
                return;

            imageBox1.Image = lastProcessedFrame;

            //skip frames due to poor performance       
            var twsDetectThread = new ThreadDetectFace(frame);           
            frameProcessing = true;
            twsdetecting = new Thread(twsDetectThread.CallDetect);
            twsdetecting.Start();

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

        private readonly Mat frame;

        // The constructor obtains the state information.
        public ThreadDetectFace(Mat frame)
        {
            this.frame = frame;            
        }

        // The thread procedure performs the task, such as formatting
        // and printing a document.
        public void CallDetect()
        {
            FaceDetect.DetectAndDisplayFunc(frame);
        }

    }
}