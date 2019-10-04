using System;
using System.Collections.Generic;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using FaceRecognition.Storage;

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
            var mat = new Mat();
            CvInvoke.Imdecode(img.Image, ImreadModes.AnyColor, mat);
            ImageGrabbed?.Invoke(mat);
        }

        public void Start()
        {
            timer.Change(TimeSpan.Zero, period);
        }
    }
}