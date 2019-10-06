using CommonObjects;
namespace FaceRecognition.Storage
{
    public class ImageLabel : IImageLabel
    {
        public int ImageLabelId { get; set; }
        public ImageLabel(string label, byte[] image)
        {
            Label = label;
            Image = image;
        }

        public string Label { get; set; }

        public byte[] Image { get; set; }
    }
}