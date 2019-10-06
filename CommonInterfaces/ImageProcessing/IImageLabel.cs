namespace CommonObjects
{
    public interface IImageLabel
    {
        byte[] Image { get; set; }
        int ImageLabelId { get; set; }
        string Label { get; set; }
    }
}