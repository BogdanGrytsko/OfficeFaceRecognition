using CommonObjects;
using System;

namespace FaceRecognitionDatabase
{
    public class LogCommand
    {
        public LogCommand() { }
        public LogCommand(CommandTypes commandType)
        {
            CommandType = commandType;
        }

        public int LogCommandId { get; set; }
        public DateTime StartCommandDate { get; set; }
        public DateTime FinishCommandDate { get; set; }
        public CommandTypes CommandType { get; }
        public CommandResults CommandResult { get; set; }
    }
}
