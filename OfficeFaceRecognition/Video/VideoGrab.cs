using System;
using System.Runtime.InteropServices;
using Emgu.CV;

namespace OfficeFaceRecognition.Video
{
    public class VideoGrab
    {
        [DllImport("msvcrt.dll")]
        public static extern int _putenv_s(string e, string v);

        private readonly VideoCapture cameraCapture;

        public event Action<Mat> ImageGrabbed;

        public VideoGrab(string videoPath)
        {
            _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "rtsp_transport;udp");
            cameraCapture = new VideoCapture(videoPath, VideoCapture.API.Ffmpeg);
            cameraCapture.ImageGrabbed += OnImageGrabbed;
        }

        public void Start()
        {
            cameraCapture.Start();
        }

        private void OnImageGrabbed(object sender, EventArgs e)
        {
            var mat = new Mat();
            if (!cameraCapture.Retrieve(mat)) return;
            ImageGrabbed?.Invoke(mat);
        }
    }
}
