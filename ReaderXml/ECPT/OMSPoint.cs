using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    public class OMSPoint : ICadastralObject
    {
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

        public string SkId { get; set; }

        public void Init(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "p_nmb":
                            {
                                PNmb = reader.ReadElementContentAsString();
                            }
                            break;
                        case "p_name":
                            {
                                PName = reader.ReadElementContentAsString();
                            }
                            break;
                        case "p_klass":
                            {
                                PKlass = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ord_x":
                            {
                                OrdX = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        case "ord_y":
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
