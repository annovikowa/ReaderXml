using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Сооружение.
    /// </summary>
    public class Construction : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Основные характеристики сооружения.
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости.
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion

        public override void Init(XmlReader reader, XsdClassifiers dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Construction
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "ObjectType":
                            {
                                if (dictionary.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out var objectType))
                                    ObjectType = objectType;
                            }
                            break;
                        case "Address":
                            {
                                var inner = reader.ReadSubtree();
                                Address = new Location(inner, dictionary.AddressRegion)?.GetAddress(false);
                                inner.Close();
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = $"{reader.Value.ToString()} руб.";
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
                        case "KeyParameter":
                            {
                                reader.MoveToAttribute("Type");
                                if (dictionary.KeyParameters.TryGetValue(reader.Value, out var keyParamenter))
                                {

                                    if (reader.Value == "05" || reader.Value == "06")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = $"{keyParamenter}: {reader.Value} кв.м.";
                                    }
                                    else if (reader.Value == "03")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = $"{keyParamenter}: {reader.Value} куб.м.";
                                    }
                                    else
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = $"{keyParamenter}: {reader.Value} м.";
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
