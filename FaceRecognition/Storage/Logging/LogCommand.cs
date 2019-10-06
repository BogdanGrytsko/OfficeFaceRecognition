using System;

namespace FaceRecognition.Storage
{
    public class LogCommand : ILogCommand
    {
        public LogCommand(CommandTypes commandType)
        {
            CommandType = commandType;
        }
        public int CommandId { get; set; }
        public DateTime StartCommandDate { get; set; }
        public DateTime FinishCommandDate { get; set; }
        public CommandTypes CommandType { get; }
        public CommandResults CommandResult { get; set; }
    }
}
