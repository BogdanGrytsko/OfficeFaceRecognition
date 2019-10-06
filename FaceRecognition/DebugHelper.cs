using System.Drawing;
using System.IO;
using CommonObjects;
using FaceRecognition.Storage;

namespace FaceRecognition
{
    public static class DebugHelper
    {
        private static int index;
        private static readonly Pen pen = new Pen(Color.Red, 2);
        public const string OutputPath = "output";

        public static void DrawFaceRectAndSave(Bitmap bmp, Rectangle rect)
        {
            var copy = new Bitmap(bmp);
            using (var g = Graphics.FromImage(copy))
            {
                g.DrawRectangle(pen, rect);
            }

            Save(copy);
        }

        public static void Save(Bitmap bmp)
        {
            new FileSystemDAL(OutputPath).Add(new ImageLabel(index++.ToString(), GetBytes(bmp) ) as IImageLabel);
        }

        private static byte[] GetBytes(Bitmap bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
