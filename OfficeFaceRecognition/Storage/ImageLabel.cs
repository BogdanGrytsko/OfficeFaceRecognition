namespace OfficeFaceRecognition.Storage
{
    public class ImageLabel
    {
        public ImageLabel(string label, byte[] image)
        {
            Label = label;
            Image = image;
        }

        public string Label { get; set; }

        public byte[] Image { get; set; }
    }
}