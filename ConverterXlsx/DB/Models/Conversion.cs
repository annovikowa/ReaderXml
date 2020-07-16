using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB.Models
{
    public class Conversion
    {
        /// <summary>
        /// можно наверно сделать Long - тут int хватит, 2 миллиарда файлов - это очень много) и всегда потом можно поменять тип
        /// </summary>
        [Key]
        public string ConversionId { get; set; } = Guid.NewGuid().ToString();

        public string PathInput { get; set; }

        public string PathOutput { get; set; }
        /// <summary>
        /// Статусы бывают только определенные, можно сделать перечисление.
        /// </summary>
        public Status Status { get; set; }

        public ICollection<Error> Errors { get; set; }
        /// <summary>
        /// Конструктор без параметров - необходим для правильной работы
        /// </summary>
        public Conversion() { }

        public Conversion(string pathInput, Status status)
        {
            PathInput = pathInput;
            Status = status;
        }
    }
}