using System.Collections.Generic;

namespace OfficeFaceRecognition.Storage
{
    public interface ITrainDataDAL
    {
        IEnumerable<ImageLabel> GetImages();
    }
}