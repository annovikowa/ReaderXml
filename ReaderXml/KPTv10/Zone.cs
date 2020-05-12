using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPTv10
{
    /// <summary>
    /// Зона.
    /// </summary>
    public class Zone : ICadastralObject
    {
        #region Свойства
        /// <summary>
        /// Учетный номер.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Вид зоны.
        /// </summary>
        public string TypeZone { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дополнительная информация.
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Координаты.
        /// </summary>
        public bool isCoordinates { get; set; }
        public string CoorSys { get; set; }
        #endregion

        public void Init(XmlReader reader, XsdClassifiers dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "AccountNumber":
                            {
                                AccountNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Description":
                            {
                                Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ContentRestrictions":
                            {
                                AdditionalInformation = reader.ReadElementContentAsString();
                            }
                            break;
                        case "TerritorialZone":
                            {
                                TypeZone = "Территориальная зона";
                            }
                            break;
                        case "SpecialZone":
                            {
                                TypeZone = "Зона с особыми условиями использования территорий";
                            }
                            break;
                        case "PermittedUse":
                            {
                                var separator = string.IsNullOrWhiteSpace(AdditionalInformation) ? "" : "; ";
                                AdditionalInformation += separator + ExtractingAdditionalInformation(reader.ReadSubtree(), dictionary);

                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                CoorSys = reader.Value.ToString();
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
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
