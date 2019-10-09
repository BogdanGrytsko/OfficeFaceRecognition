using FaceRecognitionDatabase;
using System.Collections.Generic;

namespace CommonObjects
{
    public interface ILogCommandDAL
    {
        LogCommand Get(int commandId);

        void Add(IEnumerable<LogCommand> commands);

        void Add(LogCommand command);

        void Delete(int commandId);

        void Update(LogCommand command);
    }
}