using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Пункт ОМС.
    /// </summary>
    public class OMSPointFiller : IFiller<OMSPoint>
    {
        public void Fill(OMSPoint model, XmlReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "PNmb":
                                {
                                    model.PNmb = reader.ReadElementContentAsString();
                                }
                                break;
                            case "PName":
                                {
                                    model.PName = reader.ReadElementContentAsString();
                                }
                                break;
                            case "PKlass":
                                {
                                    model.PKlass = reader.ReadElementContentAsString();
                                }
                                break;
                            case "OrdX":
                                {
                                    model.OrdX = reader.ReadElementContentAsDecimal();
                                }
                                break;
                            case "OrdY":
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
            catch (System.Exception)
            {
                //log
            }
           
        }
    }
}
