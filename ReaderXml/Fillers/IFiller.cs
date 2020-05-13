using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.Fillers
{
    /// <summary>
    /// Интерфейс заполнения модели.
    /// </summary>
    /// <typeparam name="T">Тип модели.</typeparam>
    public interface IFiller<T> where T : class
    {
        /// <summary>
        /// Заполнить модель данными.
        /// </summary>
        /// <param name="model">Модель для заполнения.</param>
        /// <param name="reader">Xml КПТ.</param>
        void Fill(T model, XmlReader reader);
    }
}
