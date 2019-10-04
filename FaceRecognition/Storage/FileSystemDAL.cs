﻿using System;
using System.Collections.Generic;
using System.IO;

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
                yield return new ImageLabel(imagePath, File.ReadAllBytes(imagePath));
            }
        }

        public ImageLabel Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(IEnumerable<ImageLabel> images)
        {
            throw new System.NotImplementedException();
        }

        public void Add(ImageLabel image)
        {
            var timedDirectory = Path.Combine(Environment.CurrentDirectory, directory, DateTime.UtcNow.ToString("yyyy.MM.dd"));
            var path = Path.Combine(timedDirectory, $"{DateTime.UtcNow:HH-mm-ss}.{image.Label}.png");
            Directory.CreateDirectory(timedDirectory);
            File.WriteAllBytes(path, image.Image);
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void SetLabel(int id, int labelId)
        {
            throw new System.NotImplementedException();
        }
    }
}