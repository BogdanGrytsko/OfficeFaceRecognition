using Emgu.CV;
using System;

namespace OfficeFaceRecognition.Video
{
    public interface IVideoGrab
    {
        event Action<Mat> ImageGrabbed;

        void Start();
    }
}
