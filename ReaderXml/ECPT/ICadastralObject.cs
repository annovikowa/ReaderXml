using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    public interface ICadastralObject
    {
        /// <summary>
        /// Система координат
        /// </summary>
        string SkId { get; set; }
        /// <summary>
        /// Инициализация кадастрового объекта
        /// </summary>
        /// <param name="reader">Экземпляр XmlReader</param>
        void Init(XmlReader reader);
    }
}
