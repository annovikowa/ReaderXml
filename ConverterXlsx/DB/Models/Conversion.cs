using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB.Models
{
    public class Conversion
    {
        public string Id { get; set; }

        public string PathInput { get; set; }

        public string PathOutput { get; set; }

        public string Status { get; set; }
    }
}