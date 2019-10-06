using System;

namespace CommonObjects
{
    public interface ILogCommand
    {
        int LogCommandId { get; set; }
        DateTime StartCommandDate { get; set; }
        DateTime FinishCommandDate { get; set; }
        CommandTypes CommandType { get; }
        CommandResults CommandResult { get; set; }
    }
}
