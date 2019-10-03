using System.Collections.Generic;

namespace FaceRecognition.Storage
{
    public interface ITrainDataDAL
    {
        IEnumerable<ImageLabel> GetImages();
    }
}