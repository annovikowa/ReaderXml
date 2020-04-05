using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.KPT
{
    public abstract class CadastralObject
    {
        /// <summary>
        /// Кадастровый номер замельного участка
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь земельного участка
        /// </summary>
        public string Area { get; set; }
    }
}
