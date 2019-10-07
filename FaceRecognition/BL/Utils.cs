using Emgu.CV;
using Emgu.CV.CvEnum;

namespace FaceRecognition.BL
{
    public static class Utils
    {
        public static byte[] GetResourceBytes(string filename)
        {
            var assembly = typeof(Utils).Assembly;

            using (var stream = assembly.GetManifestResourceStream($"FaceRecognition.{filename}"))
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        public static Mat GetMat(byte[] bytes)
        {
            var mat = new Mat();
            CvInvoke.Imdecode(bytes, ImreadModes.AnyColor, mat);
            return mat;
        }
    }
}
