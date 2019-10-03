using System;
using System.Runtime.InteropServices;
using Emgu.CV;

namespace FaceRecognition.Video
{
    public class VideoGrab : IVideoGrab
    {
        [DllImport("msvcrt.dll")]
        public static extern int _putenv_s(string e, string v);

        private readonly VideoCapture videoCapture;

        public event Action<Mat> ImageGrabbed;

        public VideoGrab(string videoPath)
            : this(new VideoCapture(videoPath, VideoCapture.API.Ffmpeg))
        {
        }

        //captures webcam
        public VideoGrab()
            : this(new VideoCapture(0))
        {
        }

        private VideoGrab(VideoCapture videoCapture)
        {
            _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "rtsp_transport;udp");
            this.videoCapture = videoCapture;
            videoCapture.ImageGrabbed += OnImageGrabbed;
        }

        public void Start()
        {
            videoCapture.Start();
        }

        private void OnImageGrabbed(object sender, EventArgs e)
        {
            var mat = new Mat();
            if (!videoCapture.Retrieve(mat)) return;
            ImageGrabbed?.Invoke(mat);
        }
    }
}
