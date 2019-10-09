using System;
using System.Collections.Generic;
using System.Threading;
using CommonObjects;
using Emgu.CV;
using FaceRecognition.BL;
using FaceRecognitionDatabase;

namespace FaceRecognition.Video
{
    public class MockVideoGrab : IVideoGrab
    {
        public event Action<Mat> ImageGrabbed;

        private readonly List<ImageLabel> images;
        private readonly Timer timer;
        private readonly TimeSpan period;
        private int currImgIdx;

        public MockVideoGrab(List<ImageLabel> images, TimeSpan period)
        {
            this.images = images;
            this.period = period;
            timer = new Timer(Callback, null, Timeout.InfiniteTimeSpan, period);
        }

        private void Callback(object state)
        {
            var img = images[currImgIdx];
            if (currImgIdx == images.Count - 1)
                currImgIdx = 0;
            else
                currImgIdx++;
            ImageGrabbed?.Invoke(Utils.GetMat(img.Image));
        }

        public void Start()
        {
            timer.Change(TimeSpan.Zero, period);
        }

        public void Pause()
        {
            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }
}