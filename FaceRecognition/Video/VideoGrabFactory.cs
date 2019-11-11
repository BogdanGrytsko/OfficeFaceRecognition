using System;
using System.Linq;
using System.Runtime.InteropServices;
using FaceRecognition.Storage;

namespace FaceRecognition.Video
{
    public class VideoGrabFactory
    {
        [DllImport("msvcrt.dll")]
        public static extern int _putenv_s(string e, string v);

        private const string oldCamera = "rtsp://admin:Face1234@192.168.5.49:554/onvif1", newCamera = "rtsp://192.168.5.5:8554/mjpeg/1";

        public static IVideoGrab GetOldCamera()
        {
            _putenv_s("OPENCV_FFMPEG_CAPTURE_OPTIONS", "rtsp_transport;udp"); // setup environment to decode RTSP UDP before videograbber creating 
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