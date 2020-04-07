using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    //Дополнительная информация. PermittedUse не понятно 
    public class Zone : ICadastralObject
    {
        #region
        /// <summary>
        /// Учетный номер
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Вид зоны
        /// </summary>
        public string TypeZone { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        private string PermitedAncillary { get; set; }
        #endregion

        public Zone()
        {
        }

        public void Init(XmlReader reader, Dictionary dictionary)
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
                                AdditionalInformation = ExtractingAdditionalInformation(reader, dictionary, AdditionalInformation);
                                if (reader.LocalName == "PermitedAncillary")
                                    goto case "PermitedAncillary";
                            }
                            break;
                        case "PermitedAncillary":
                            {
                                PermitedAncillary = ExtractingAdditionalInformation(reader, dictionary, PermitedAncillary);
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out var entSys))
                                    EntSys = entSys;
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

        private string ExtractingAdditionalInformation(XmlReader reader, Dictionary dictionary, string additionalInformation)
        {
            string perminttedUse = "", landUse = "", utilization = "";
            if (reader.ReadToFollowing("TypePermittedUse"))
            {
                dictionary.PermitUse.TryGetValue(reader.ReadElementContentAsString(), out var str);
                additionalInformation += $"{str}: ";
            }
            reader.MoveToContent();
            if (reader.LocalName == "LandUse")
            {
                dictionary.LandUse.TryGetValue(reader.ReadElementContentAsString(), out var str);
                landUse = $"{str}; ";
                reader.MoveToContent();
            }
            if (reader.LocalName == "Utilization")
            {
                dictionary.Utilization.TryGetValue(reader.ReadElementContentAsString(), out var str);
                utilization = $"{str}; ";
                reader.MoveToContent();
            }
            if (reader.LocalName == "PermittedUse")
            {
                perminttedUse += $"{reader.ReadElementContentAsString()}; ";
                reader.MoveToContent();
            }

            if (!String.IsNullOrEmpty(perminttedUse))
                additionalInformation += perminttedUse;
            else if (!String.IsNullOrEmpty(landUse))
                additionalInformation += landUse;
            else if (!String.IsNullOrEmpty(utilization))
                additionalInformation += utilization;
            return additionalInformation;
        }
    }
}
