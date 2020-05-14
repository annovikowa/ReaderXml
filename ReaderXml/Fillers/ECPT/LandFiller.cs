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
    /// Земельный участок.
    /// </summary>
    public class LandFiller : IFiller<Parcel>
    {
        public void Fill(Parcel model, XmlReader reader)
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
                                reader.ReadToDescendant("value");
                                model.Area = $"{reader.ReadElementContentAsString()} кв. м.";
                            }
                            break;
                        case "subtype":
                            {
                                reader.ReadToDescendant("value");
                                model.Name = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address_location":
                            {
                                model.Address = new Address(reader.ReadSubtree()).GetAddress(true);
                            }
                            break;
                        case "category":
                            {
                                reader.ReadToDescendant("value");
                                model.Category = reader.ReadElementContentAsString();
                            }
                            break;
                        case "permitted_use_established":
                            {
                                model.Utilization = FillPermittedUse(reader.ReadSubtree());
                            }
                            break;
                        case "permitted_uses_grad_reg":
                            {
                                if (string.IsNullOrEmpty(model.Utilization))
                                    model.Utilization = FillPermittedUse(reader.ReadSubtree());
                            }
                            break;
                        case "common_land_cad_number":
                            {
                                reader.ReadToDescendant("cad_number");
                                model.ParentCadastralNumbers = reader.ReadElementContentAsString();
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

        /// <summary>
        /// Возвращает вид разрешенного использования.
        /// </summary>
        /// <param name="reader">XmlReader узла вида разрешенного использования.</param>
        /// <returns>Вид разрешенного использования.</returns>
        private string FillPermittedUse(XmlReader reader)
        {
            string LandUseMer = "";
            string ByDocument = "";
            string LandUse = "";
            string PermittedUseText = "";
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "land_use_mer":
                            {
                                reader.ReadToDescendant("value");
                                LandUseMer = reader.ReadElementContentAsString();
                            }
                            break;
                        case "by_document":
                            {
                                ByDocument = reader.ReadElementContentAsString();
                            }
                            break;
                        case "land_use":
                            {
                                reader.ReadToDescendant("value");
                                LandUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "permitted_use_text":
                            {
                                PermittedUseText = reader.ReadElementContentAsString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();
            return new[] { LandUseMer, ByDocument, LandUse, PermittedUseText }.FirstOrDefault(x => !string.IsNullOrEmpty(x));
            
        }
    }
}
