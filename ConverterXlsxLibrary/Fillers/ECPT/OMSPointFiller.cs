using System.Xml;
using System;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.ECPT
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
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }
            
        }
    }
}
