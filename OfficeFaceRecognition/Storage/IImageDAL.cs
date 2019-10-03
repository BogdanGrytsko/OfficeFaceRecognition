using System.Collections.Generic;

namespace OfficeFaceRecognition.Storage
{
    public interface IImageDAL
    {
        ImageLabel Get(int id);

        void Add(IEnumerable<ImageLabel> images);

        void Add(ImageLabel image);

        void Delete(int id);

        void SetLabel(int id, int labelId);
    }
}