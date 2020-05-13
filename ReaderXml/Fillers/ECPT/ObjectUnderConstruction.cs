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
    /// Объект незавершенного строительства (ОНС).
    /// </summary>
    public class ObjectUnderConstruction : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Основные характеристики ОНС.
        /// </summary>
        public string BaseParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости.
        /// </summary>
        public string Purpose { get; set; }

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
                        case "purpose":
                            {
                                Purpose = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address_location":
                            {
                                Address += new Address(reader.ReadSubtree()).GetAddress(false);
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
                        case "base_parameter":
                            {
                                FillBaseParameters(reader.ReadSubtree());
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Считывает основные характеристики сооружения.
        /// </summary>
        /// <param name="reader">XmlReader узла основных характеристик.</param>
        private void FillBaseParameters(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "area":
                            {
                                BaseParameters += $"Площадь: {reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
                        case "built_up_area":
                            {
                                BaseParameters += $"Площадь: {reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
                        case "extension":
                            {
                                BaseParameters += $"Протяженность: {reader.ReadElementContentAsString()} м.";
                            }
                            break;
                        case "depth":
                            {
                                BaseParameters += $"Глубина: {reader.ReadElementContentAsString()} м.";
                            }
                            break;
                        case "occurence_depth":
                            {
                                BaseParameters += $"Глубина залегания: {reader.ReadElementContentAsString()} м.";
                            }
                            break;
                        case "volume":
                            {
                                BaseParameters += $"Объем: {reader.ReadElementContentAsString()} куб.м.";
                            }
                            break;
                        case "height":
                            {
                                BaseParameters += $"Высота: {reader.ReadElementContentAsString()} м.";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();
        }
    }
}
