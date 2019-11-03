using CommonObjects;
using FaceRecognitionDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FaceRecognition.Storage
{
    public class FileSystemDAL : ITrainDataDAL, IImageDAL
    {
        private readonly string directory;

        public FileSystemDAL(string directory)
        {
            this.directory = directory;
        }

        public IEnumerable<ImageLabel> GetImages()
        {
            var imagePaths = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            foreach (var imagePath in imagePaths)
            {
                var label = Path.GetFileName(Path.GetDirectoryName(imagePath));
                yield return new ImageLabel(label, File.ReadAllBytes(imagePath));
            }
        }

        public Dictionary<string, int> GetLabelMap()
        {
            var dirs = Directory.GetDirectories(directory);
            var idx = 1;
            var dic = dirs.Select(Path.GetFileName).ToDictionary(n => n, n => idx++);
            dic["unknown"] = 0;
            return dic;
        }

        public ImageLabel Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<ImageLabel> images)
        {
            throw new NotImplementedException();
        }

        public void Add(ImageLabel image)
        {
            var timedDirectory = Path.Combine(Environment.CurrentDirectory, directory, image.Label);
            var path = Path.Combine(timedDirectory, $"{DateTime.UtcNow:HH-mm-ss-fff}.{image.Label}.png");
            Directory.CreateDirectory(timedDirectory);
            File.WriteAllBytes(path, image.Image);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void SetLabel(int imageLabelId, string label)
        {
            throw new NotImplementedException();
        }
    }
}