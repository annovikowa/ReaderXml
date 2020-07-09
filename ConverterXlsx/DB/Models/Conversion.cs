using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB.Models
{
    public class Conversion
    {
        /// <summary>
        /// можно наверно сделать Long
        /// </summary>
        public string Id { get; set; }

        public string PathInput { get; set; }

        public string PathOutput { get; set; }
        /// <summary>
        /// Статусы бывают только определенные, можно сделать перечисление.
        /// </summary>
        public string Status { get; set; }
        public Conversion(string id, string pathInput, string status)
        {
            Id = id;
            PathInput = pathInput;
            Status = status;
        }
    }
}