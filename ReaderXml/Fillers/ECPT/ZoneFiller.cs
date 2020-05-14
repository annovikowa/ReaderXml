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
    /// Зона.
    /// </summary>
    public class ZoneFiller : IFiller<Zone>
    {
        public void Fill(Zone model, XmlReader reader)
        {
            string NameByDoc = "";
            string TypeZone = "";
            string Number = "";
            string Index = "";
            string WaterObjectName = "";
            string WaterObjectType = "";
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "reg_numb_border":
                            {
                                model.AccountNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "registration_date":
                            {
                                model.RegistrationDate = reader.ReadElementContentAsString();
                            }
                            break;
                        case "type_boundary":
                            {
                                reader.ReadToDescendant("value");
                                model.Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "type_zone":
                            {
                                reader.ReadToDescendant("value");
                                model.TypeZone = reader.ReadElementContentAsString();
                                TypeZone += $"тип зоны: {model.TypeZone}";
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
                        case "name_by_doc":
                            {
                                NameByDoc += $"Наименование по документу: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "number":
                            {
                                Number += $"№{reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "index":
                            {
                                Index += $"Индекс: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "water_object_name":
                            {
                                WaterObjectName += $"Наименование водного объекта: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "water_object_type":
                            {
                                reader.ReadToDescendant("value");
                                WaterObjectType += $"Вид водного объекта: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            var text = new[] { NameByDoc, TypeZone, Number, Index, WaterObjectName, WaterObjectType }.Where(x => !string.IsNullOrEmpty(x));
            model.AdditionalInformation = string.Join(". ", text);
        }     
    }
}
