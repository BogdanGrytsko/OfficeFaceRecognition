using Emgu.CV;

namespace FaceRecognition.Storage
{
    public class FaceDAL
    {
        public void Save(Mat mat)
        {
            DebugHelper.Save(mat.Bitmap);
        }
    }
}
