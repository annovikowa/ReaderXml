using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Зона.
    /// </summary>
    public class ZoneFiller : IFiller<Zone>
    {
        public void Fill(Zone model, XmlReader reader)
        {
            try
            {
                var xsdDictionaries = XsdClassifiers.GetInstance();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "AccountNumber":
                                {
                                    model.AccountNumber = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Description":
                                {
                                    model.Description = reader.ReadElementContentAsString();
                                }
                                break;
                            case "ContentRestrictions":
                                {
                                    model.AdditionalInformation = reader.ReadElementContentAsString();
                                }
                                break;
                            case "TerritorialZone":
                                {
                                    model.TypeZone = "Территориальная зона";
                                }
                                break;
                            case "SpecialZone":
                                {
                                    model.TypeZone = "Зона с особыми условиями использования территорий";
                                }
                                break;
                            case "PermittedUse":
                                {
                                    var separator = string.IsNullOrWhiteSpace(model.AdditionalInformation) ? "" : "; ";
                                    model.AdditionalInformation += separator + ExtractingAdditionalInformation(reader.ReadSubtree(), xsdDictionaries);

                                }
                                break;
                            case "EntitySpatial":
                                {
                                    reader.MoveToAttribute("EntSys");
                                    model.CoorSys = reader.Value.ToString();
                                }
                                break;
                            case "Ordinate":
                                model.HasCoordinates = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log
            }
            
        }

        /// <summary>
        /// Получение дополнительной информации о зонах.
        /// </summary>
        /// <param name="reader">XmlReader узла видов разрешенных использований.</param>
        /// <param name="dictionary">Словарь для перевода кодов в значения по схеме.</param>
        /// <returns>Дополнительную информацию о зонах.</returns>
        private string ExtractingAdditionalInformation(XmlReader reader, XsdClassifiers dictionary)
        {
            var type = "";
            var permittedUseText = "";
            var landUse = "";
            var utilization = "";
            var ancillaries = new List<string>();
            reader.Read();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "TypePermittedUse":
                            {
                                dictionary.PermitUse.TryGetValue(reader.ReadElementContentAsString(), out type);
                                break;
                            }
                        case "PermittedUse":
                            {
                                permittedUseText = reader.ReadElementContentAsString();
                                break;
                            }
                        case "LandUse":
                            {
                                dictionary.LandUse.TryGetValue(reader.ReadElementContentAsString(), out landUse);
                                break;
                            }
                        case "Utilization":
                            {
                                dictionary.Utilization.TryGetValue(reader.ReadElementContentAsString(), out utilization);
                                break;
                            }
                        case "PermitedAncillary":
                            {
                                ancillaries.Add(ExtractingAdditionalInformation(reader.ReadSubtree(), dictionary));
                                break;
                            }
                    }
                }
            }
            reader.Close();
            var text = new[] { permittedUseText, landUse, utilization }.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            var additional = string.Join("; ", ancillaries);            
            return $"{type}, {text}{(string.IsNullOrWhiteSpace(additional) ? "" : $" (дополнительно: {additional})")}";
        }
    }
}
