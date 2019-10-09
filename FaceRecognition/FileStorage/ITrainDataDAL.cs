using FaceRecognitionDatabase;
using System.Collections.Generic;

namespace CommonObjects
{
    public interface ITrainDataDAL
    {
        IEnumerable<ImageLabel> GetImages();

        Dictionary<string, int> GetLabelMap();
    }
}