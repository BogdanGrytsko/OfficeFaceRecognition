using System;

namespace FaceRecognition.Storage
{
    public class LogRecognition : ILogRecognition
    {
        public int Id { get; }
        public int RecognizedUserId { get; }
        public DateTime RecognitionDate { get; }
        public ImageLabel RecognizedImageLabel { get; set; }
        public double RecognitionConfidance { get; }
    }
}
