using ReaderXml.KPTv10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml
{
    public abstract class CadastralObject : ICadastralObject
    {
        /// <summary>
        /// Кадастровый номер замельного участка.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь земельного участка.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Наличие координат.
        /// </summary>
        public bool isCoordinates { get; set; }
        public string CoorSys { get; set; }

        public abstract void Init(XmlReader reader, XsdClassifiers dictionary);
    }
}
