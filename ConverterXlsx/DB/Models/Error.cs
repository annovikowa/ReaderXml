using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB.Models
{
    public class Error
    {
        // int или long выбрать?
        public long Id { get; set; }
        public Conversion Conversion { get; set; }
        public string ErrorDescription { get; set; }
    }
}