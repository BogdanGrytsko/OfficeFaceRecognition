using System;

namespace FaceRecognitionDatabase
{
    public class LogRecognition
    {
        public int LogRecognitionId { get; set; }
        public int RecognizedUserId { get; }
        public DateTime RecognitionDate { get; }

        public ImageLabel ImageLabel { get; set; }

        public double RecognitionConfidance { get; set; }
    }
}
