using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.KPTv10
{
    public class XsdClassifiers
    {
        public Dictionary<string, string> AddressRegion { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsName { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsCategory { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Utilization { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LandUse { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ObjectType { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> KeyParameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> PermitUse { get; set; } = new Dictionary<string, string>();
    }
}
