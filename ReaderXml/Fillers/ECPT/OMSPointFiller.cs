using ReaderXml.Fillers;
using ReaderXml.KPT;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    /// <summary>
    /// Пункт ОМС.
    /// </summary>
    public class OMSPointFiller : IFiller<OMSPoint>
    {
        public void Fill(OMSPoint model, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "p_nmb":
                            {
                                model.PNmb = reader.ReadElementContentAsString();
                            }
                            break;
                        case "p_name":
                            {
                                model.PName = reader.ReadElementContentAsString();
                            }
                            break;
                        case "p_klass":
                            {
                                model.PKlass = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ord_x":
                            {
                                model.OrdX = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        case "ord_y":
                            {
                                model.OrdY = reader.ReadElementContentAsDecimal();
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
