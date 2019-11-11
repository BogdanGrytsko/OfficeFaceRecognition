using System;
using Emgu.CV;

namespace FaceRecognition.Video
{
    public class VideoGrab : IVideoGrab
    {       
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
            this.videoCapture = videoCapture;
            videoCapture.ImageGrabbed += OnImageGrabbed;
        }

        public void Start()
        {
            videoCapture.Start();
        }

        public void Pause()
        {
            videoCapture.Pause();
        }

        private void OnImageGrabbed(object sender, EventArgs e)
        {
            var mat = new Mat();
            if (!videoCapture.Retrieve(mat)) return;
            ImageGrabbed?.Invoke(mat);
        }
    }
}
