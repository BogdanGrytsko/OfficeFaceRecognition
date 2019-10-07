using System.Collections.Generic;
using System.Linq;

namespace FaceRecognition.BL
{
    public class LabelMap
    {
        public LabelMap(Dictionary<string, int> map)
        {
            Map = map;
            ReverseMap = map.ToDictionary(k => k.Value, k => k.Key);
        }

        public Dictionary<string, int> Map { get; set; }

        public Dictionary<int, string> ReverseMap { get; set; }
    }
}