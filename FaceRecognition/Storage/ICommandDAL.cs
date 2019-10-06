using System.Collections.Generic;

namespace FaceRecognition.Storage
{
    public interface ILogCommandDAL
    {
        ILogCommand Get(int commandId);

        void Add(IEnumerable<ILogCommand> commands);

        void Add(ILogCommand command);

        void Delete(int commandId);

        void Update(ILogCommand command);
    }
}