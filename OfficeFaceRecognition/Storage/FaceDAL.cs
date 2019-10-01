using Emgu.CV;

namespace OfficeFaceRecognition.Storage
{
    public class FaceDAL
    {
        public void Save(Mat mat)
        {
            DebugHelper.Save(mat.Bitmap);
        }
    }
}
