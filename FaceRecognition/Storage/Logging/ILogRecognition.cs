using System;

namespace FaceRecognition.Storage
{
    public interface ILogRecognition
    {
        int Id { get; }
        int RecognizedUserId { get; }
        DateTime RecognitionDate { get; } 
        ImageLabel RecognizedImageLabel { get; set; }
        double RecognitionConfidance { get; }
    }
}
