using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class OMSPoint : ICadastralObject
    {
        #region
        /// <summary>
        /// Номер пункта опорной межевой сети на плане
        /// </summary>
        public string PNmb { get; set; }

        /// <summary>
        /// Название и тип пункта
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// Класс геодезической сети
        /// </summary>
        public string PKlass { get; set; }

        /// <summary>
        /// Координата Х
        /// </summary>
        public decimal OrdX { get; set; }

        /// <summary>
        /// Координата У
        /// </summary>
        public decimal OrdY { get; set; }
        #endregion

        public OMSPoint()
        {
        }

        public void Init(XmlReader reader, Dictionary dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "PNmb":
                            {
                                PNmb = reader.ReadElementContentAsString();
                            }
                            break;
                        case "PName":
                            {
                                PName = reader.ReadElementContentAsString();
                            }
                            break;
                        case "PKlass":
                            {
                                PKlass = reader.ReadElementContentAsString();
                            }
                            break;
                        case "OrdX":
                            {
                                OrdX = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        case "OrdY":
                            {
                                OrdY = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
