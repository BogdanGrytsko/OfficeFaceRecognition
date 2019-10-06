using CommonObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceRecognitionDatabase
{
    public class LogRecognition : ILogRecognition
    {
        public int LogRecognitionId { get; set; }
        public int RecognizedUserId { get; }
        public DateTime RecognitionDate { get; }

        public ImageLabel ImageLabel { get; set; }

        [NotMapped]
        IImageLabel ILogRecognition.ImageLabel { get => ImageLabel; set => ImageLabel = value as ImageLabel; }

        public double RecognitionConfidance { get; set; }
    }
}
