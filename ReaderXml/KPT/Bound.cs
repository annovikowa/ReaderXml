using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class Bound : ICadastralObject
    {
        #region
        /// <summary>
        /// Учетный номер
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Вид границы
        /// </summary>
        public string TypeBoundary { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInformation { get; set; }
        #endregion

        public Bound()
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
                        case "NameNeighbours":
                            {
                                TypeBoundary = "Граница между субъектами Российской Федерации";
                                if (String.IsNullOrEmpty(AdditionalInformation))
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "MunicipalBoundary":
                            {
                                TypeBoundary = "Граница муниципального образования";
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                if (String.IsNullOrEmpty(AdditionalInformation))
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "InhabitedLocalityBoundary":
                            {
                                TypeBoundary = "Граница населенного пункта";
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                if (String.IsNullOrEmpty(AdditionalInformation))
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "Description":
                            {
                                Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                EntSys = reader.Value.ToString();
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
    }
}
