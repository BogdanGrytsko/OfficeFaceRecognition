using CommonObjects;

namespace FaceRecognitionDatabase
{
    public class UserLabel: IUserLabel
    {
        public int UserLabelId { get; set; }
        public string Label { get; set; }
    }
}
