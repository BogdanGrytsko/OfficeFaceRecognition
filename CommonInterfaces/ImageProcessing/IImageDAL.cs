using System.Collections.Generic;

namespace CommonObjects
{
    public interface IImageDAL
    {
        IImageLabel Get(int id);

        void Add(IEnumerable<IImageLabel> images);

        void Add(IImageLabel image);

        void Delete(int id);

        void SetLabel(int id, int labelId);
    }
}