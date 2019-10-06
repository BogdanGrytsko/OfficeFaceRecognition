namespace FaceRecognition.Storage
{
    public interface IFaceRecognitionUser
    {
        int UserId { get; set; }
        CommandTypes DefaultCommand { get; set; }
        ILabel UserLabel { get; set; }
    }
}
