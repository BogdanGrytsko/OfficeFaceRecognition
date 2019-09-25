using System.Collections.Generic;
using System.Linq;

namespace OfficeFaceRecognition.BL
{
    public class LabelMap
    {
        public LabelMap(IEnumerable<string> names)
        {
            var idx = 1;
            Map = names.Distinct().ToDictionary(n => n, n => idx++);
            ReverseMap = Map.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public Dictionary<string, int> Map { get; set; }

        public Dictionary<int, string> ReverseMap { get; set; }
    }
}