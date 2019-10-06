using CommonObjects;
using FaceRecognition.Storage;
using System;

namespace FaceRecognition.BL
{
    public abstract class CommandProcessorBase
    {
        public CommandProcessorBase(ILogCommandDAL commandDAL)
        {
            LogCommandDAL = commandDAL;
        }

        protected ILogCommandDAL LogCommandDAL;

        /// <summary>
        /// Check user rights and execute command with logging
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="user"></param>
        /// <param name="CommandExecutor"></param>
        /// <returns></returns>
        public virtual ILogCommand ExecuteNewCommand(CommandTypes commandType, IFaceRecognitionUser user, Func<CommandTypes, CommandResults> CommandExecutor = null)
        {
            var command = new LogCommand(commandType);
            command.StartCommandDate = DateTime.UtcNow;
            LogCommandDAL.Add(command);

            if (!IsExecutionAllowed(user))
            {
                command.CommandResult = CommandResults.Fail;
                return command;
            }

            if (CommandExecutor == null)
            {
                command.FinishCommandDate = DateTime.UtcNow;
                command.CommandResult = CommandResults.Undefined;
                LogCommandDAL.Update(command);
                return command;
            }

            if (command.CommandType == CommandTypes.DoNothingLogOnly)
            {
                command.FinishCommandDate = DateTime.UtcNow;
                command.CommandResult = CommandResults.Success;
                LogCommandDAL.Update(command);
                return command;
            }

            var result = CommandExecutor(commandType);
            command.CommandResult = result;
            command.FinishCommandDate = DateTime.UtcNow;
            LogCommandDAL.Update(command);
            return command;
        }

        /// <summary>
        /// Override this to check user rights 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool IsExecutionAllowed(IFaceRecognitionUser user)
        {
            return true;
        }
    }
}
