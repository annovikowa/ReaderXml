using ReaderXml.Fillers;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Кадастровый план территории.
    /// </summary>
    public class KPTFiller : IFiller<CadastralPlanTerritory>
    {

        public void Fill(CadastralPlanTerritory model, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "CadastralBlock":
                            {
                                var inner = reader.ReadSubtree();
                                var cadBlock = new CadastralBlock();
                                var filler = new CadastralBlockFiller.Fill(cadBlock, inner);
                                model.CadastralBlocks.Add(cadBlock);
                                inner.Close();
                            }
                            break;
                        case "Organization":
                            {
                                model.OrganRegistrRights = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Date":
                            {
                                model.DateFormation = reader.ReadElementContentAsDateTime();
                            }
                            break;
                        case "Number":
                            {
                                model.RegistrationNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Appointment":
                            {
                                model.Official += $"{reader.ReadElementContentAsString()}, ";
                            }
                            break;
                        case "FamilyName":
                            {
                                model.Official += $"{reader.ReadElementContentAsString()} ";
                            }
                            break;
                        case "FirstName":
                            {
                                model.Official += $"{reader.ReadElementContentAsString()} ";
                            }
                            break;
                        case "Patronymic":
                            {
                                model.Official += $"{reader.ReadElementContentAsString()}";
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
