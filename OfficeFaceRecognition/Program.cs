using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CommandLine;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using OfficeFaceRecognition.Detector;

namespace OfficeFaceRecognition
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<FaceRecognitionParams>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        private static void RunOptions(FaceRecognitionParams facePars)
        {
            var detectionModule = new DetectionModule(facePars);
            var faces = detectionModule.GetLabeledFaces();
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
