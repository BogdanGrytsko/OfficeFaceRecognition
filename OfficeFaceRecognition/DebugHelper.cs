using System;
using System.Drawing;
using System.IO;

namespace OfficeFaceRecognition
{
    public static class DebugHelper
    {
        private static int index;
        private static readonly Pen pen = new Pen(Color.Red, 2);
        private const string OutputPath = "output";

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

            var path = Path.Combine(Environment.CurrentDirectory, $"{OutputPath}\\{index++}.png");
            copy.Save(path);
        }
    }
}
