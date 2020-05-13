using ReaderXml.KPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml
{
    public abstract class CadastralObject
    {
        /// <summary>
        /// Наличие координат.
        /// </summary>
        public bool HasCoordinates { get; set; }
        /// <summary>
        /// Система координат.
        /// </summary>
        public string CoorSys { get; set; }
    }
}
