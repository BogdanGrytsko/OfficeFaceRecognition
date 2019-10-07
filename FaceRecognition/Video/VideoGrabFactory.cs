using System;
using System.Linq;
using FaceRecognition.Storage;

namespace FaceRecognition.Video
{
    public class VideoGrabFactory
    {
        private const string oldCamera = "rtsp://admin:Face1234@192.168.5.49:554/onvif1", newCamera = "rtsp://192.168.5.5:8554/mjpeg/1";

        public static IVideoGrab GetOldCamera()
        {
            return new VideoGrab(oldCamera);
        }

        public static IVideoGrab GetNewCamera()
        {
            return new VideoGrab(newCamera);
        }

        public static IVideoGrab GetSelfCamera()
        {
            return new VideoGrab();
        }

        public static IVideoGrab GetMockCamera(string path)
        {
            var trainDataDAL = new FileSystemDAL(path);
            return new MockVideoGrab(trainDataDAL.GetImages().Take(100).ToList(), TimeSpan.FromMilliseconds(250));
        }
    }
}