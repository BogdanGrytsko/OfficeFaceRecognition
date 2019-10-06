using System;

namespace FaceRecognition.Storage
{
    public interface ILogCommand
    {
        int CommandId { get; }
        DateTime StartCommandDate { get; set; }
        DateTime FinishCommandDate { get; set; }
        CommandTypes CommandType { get; }
        CommandResults CommandResult { get; set; }
    }
}
