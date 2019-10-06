using System;

namespace CommonObjects
{
    public interface ILogRecognition
    {
        int LogRecognitionId { get; }
        int RecognizedUserId { get; }
        DateTime RecognitionDate { get; }
        IImageLabel ImageLabel { get; set; }
        double RecognitionConfidance { get; }
    }
}
