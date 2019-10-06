using CommonObjects;

namespace FaceRecognitionDatabase
{
    public class FaceRecognitionUser: IFaceRecognitionUser
    {
        public int UserId { get; set; }
        public CommandTypes DefaultCommand { get; set; }
        public IUserLabel UserLabel { get; set; }
    } 
}
