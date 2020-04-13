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
        string EntSys { get; set; }
        /// <summary>
        /// Инициализация кадастрового объекта
        /// </summary>
        /// <param name="reader">Экземпляр XmlReader</param>
        /// <param name="dictionary">Необходимый словарь</param>
        void Init(XmlReader reader, KPT.Dictionary dictionary);
    }
}
