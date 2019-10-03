using System;
using System.Drawing;
using System.IO;

namespace FaceRecognition
{
    public static class DebugHelper
    {
        private static int index;
        private static readonly Pen pen = new Pen(Color.Red, 2);
        public const string OutputPath = "output";

        static DebugHelper()
        {
            Directory.CreateDirectory(OutputPath);
        }

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
            var directory = Path.Combine(Environment.CurrentDirectory, OutputPath, DateTime.UtcNow.ToShortDateString());
            var path = Path.Combine(directory, $"{index++}.{Guid.NewGuid()}.png");
            Directory.CreateDirectory(directory);
            bmp.Save(path);
        }
    }
}
