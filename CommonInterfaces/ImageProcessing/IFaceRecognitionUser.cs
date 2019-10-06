namespace CommonObjects
{
    public interface IFaceRecognitionUser
    {
        int UserId { get; set; }
        CommandTypes DefaultCommand { get; set; }
        IUserLabel UserLabel { get; set; }
    }
}
