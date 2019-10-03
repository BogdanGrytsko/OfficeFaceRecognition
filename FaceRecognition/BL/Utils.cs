namespace FaceRecognition.BL
{
    internal static class Utils
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
    }
}
