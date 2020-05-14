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
    /// Здание.
    /// </summary>
    public class BuildFiller : IFiller<Building>
    {
        public void Fill(Building model, XmlReader reader)
        {
            reader.Read();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "common_data":
                            {
                                reader.ReadToDescendant("cad_number");
                                model.CadastralNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "area":
                            {
                                model.Area = $"{reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
                        case "purpose":
                            {
                                reader.ReadToDescendant("value");
                                model.ObjectType = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address":
                            {
                                model.Address += $"{new Address(reader.ReadSubtree()).GetAddress(false)}; ";
                            }
                            break;
                        case "location":
                            {
                                model.Address += $"{new Address(reader.ReadSubtree()).GetAddress(false)}; ";
                            }
                            break;
                        case "permitted_use":
                            {
                                reader.ReadToDescendant("name");
                                model.PermittedUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "united_cad_number":
                            {
                                reader.ReadToDescendant("cad_number");
                                model.UnitedCadNumbers = reader.ReadElementContentAsString();
                            }
                            break;
                        case "cost":
                            {
                                reader.ReadToDescendant("value");
                                model.CadastralCost = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                model.CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            model.HasCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
