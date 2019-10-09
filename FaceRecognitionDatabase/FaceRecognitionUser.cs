using CommonObjects;

namespace FaceRecognitionDatabase
{
    public class FaceRecognitionUser
    {
        public int UserId { get; set; }
        public CommandTypes DefaultCommand { get; set; }
        public UserLabel UserLabel { get; set; }
    } 
}
