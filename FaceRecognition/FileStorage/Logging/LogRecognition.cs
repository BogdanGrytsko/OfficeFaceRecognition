using CommonObjects;
using System;

namespace FaceRecognition.Storage
{
    public class LogRecognition : ILogRecognition
    {
        public int LogRecognitionId { get; }
        public int RecognizedUserId { get; }
        public DateTime RecognitionDate { get; }
        public IImageLabel ImageLabel { get; set; }
        public double RecognitionConfidance { get; }
    }
}
