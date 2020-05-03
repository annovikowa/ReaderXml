using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    public class Build : CadastralObject, ICadastralObject
    {
        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        public string SkId { get; set; }

        /// <summary>
        /// Кадастровый номер ЕНК
        /// </summary>
        public string UnitedCadNumbers { get; set; }

        /// <summary>
        /// Назначение здания
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// Вид разрешенного использования
        /// </summary>
        public string PermittedUse { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }

        public void Init(XmlReader reader)
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
                                reader.MoveToContent();
                                reader.ReadToDescendant("cad_number");
                                CadastralNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "area":
                            {
                                Area = $"{reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
                        case "purpose":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("value");
                                Purpose = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address_location":
                            {
                                
                            }
                            break;
                        case "permitted_use":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("name");
                                PermittedUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "united_cad_number":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("cad_number");
                                UnitedCadNumbers = reader.ReadElementContentAsString();
                            }
                            break;
                        case "cost":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("value");
                                CadastralCost = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("sk_id");
                                SkId = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
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
