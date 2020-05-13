using ReaderXml.KPT;
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
    public class Build : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Кадастровый номер ЕНК.
        /// </summary>
        public string UnitedCadNumbers { get; set; }

        /// <summary>
        /// Назначение здания.
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// Вид разрешенного использования.
        /// </summary>
        public string PermittedUse { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion

        public override void Init(XmlReader reader, XsdClassifiers dictionary = null)
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
                                reader.ReadToDescendant("value");
                                Purpose = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address":
                            {
                                Address += $"{new Address(reader.ReadSubtree()).GetAddress(false)}; ";
                            }
                            break;
                        case "location":
                            {
                                Address += $"{new Address(reader.ReadSubtree()).GetAddress(false)}; ";
                            }
                            break;
                        case "permitted_use":
                            {
                                reader.ReadToDescendant("name");
                                PermittedUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "united_cad_number":
                            {
                                reader.ReadToDescendant("cad_number");
                                UnitedCadNumbers = reader.ReadElementContentAsString();
                            }
                            break;
                        case "cost":
                            {
                                reader.ReadToDescendant("value");
                                CadastralCost = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            HasCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
