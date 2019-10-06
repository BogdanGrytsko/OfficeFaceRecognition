using System.Collections.Generic;

namespace CommonObjects
{
    public interface ITrainDataDAL
    {
        IEnumerable<IImageLabel> GetImages();
    }
}