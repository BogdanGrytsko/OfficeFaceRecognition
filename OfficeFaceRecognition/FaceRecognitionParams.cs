using CommandLine;

namespace OfficeFaceRecognition
{
    public class FaceRecognitionParams
    {
        [Option('i', "DataSet", Required = true, HelpText = "path to input directory of faces + images")]
        public string DataSet { get; set; }

        [Option('e', "Embeddings", Required = true, HelpText = "path to output serialized db of facial embeddings")]
        public string Embeddings { get; set; }

        [Option('d', "Detector", Required = true, HelpText = "path to OpenCV's deep learning face detector")]
        public string Detector { get; set; }

        [Option('m', "EmbeddingModel", Required = true, HelpText = "path to OpenCV's deep learning face embedding model")]
        public string EmbeddingModel { get; set; }

        [Option('c', "Confidence", HelpText = "minimum probability to filter weak detections", Default = 0.5)]
        public double Confidence { get; set; }
    }
}