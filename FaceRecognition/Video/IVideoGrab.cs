using Emgu.CV;
using System;

namespace FaceRecognition.Video
{
    public interface IVideoGrab
    {
        event Action<Mat> ImageGrabbed;

        void Start();
    }
}
