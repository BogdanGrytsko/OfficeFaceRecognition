using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeFaceRecognition
{
    static class DebugHelper
    {
        public static int index = 0;
        public static Pen pan = new Pen(Color.Red, 2);
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
                g.DrawRectangle(pan, rect);
            }

            var path = Path.Combine(Environment.CurrentDirectory, $"{OutputPath}\\{index++}.png");

            copy.Save(path);
        }
    }
}
