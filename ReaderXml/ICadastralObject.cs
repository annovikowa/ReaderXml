using ReaderXml.KPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml
{
    public interface ICadastralObject
    {
        /// <summary>
        /// Система координат.
        /// </summary>
        string CoorSys { get; set; }

        /// <summary>
        /// Инициализация кадастрового объекта.
        /// </summary>
        /// <param name="reader">Экземпляр XmlReader.</param>
        /// <param name="dictionary">Экзепляр словарей для перевода кодов в значения по схеме.</param>
        void Init(XmlReader reader, XsdClassifiers dictionary);
    }
}
