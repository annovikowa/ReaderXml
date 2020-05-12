using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPTv10
{
    /// <summary>
    /// Здание.
    /// </summary>
    public class Building : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Назначение здания.
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
            #region Присваиваем атрибуты Building
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
                        case "Area":
                            {
                                Area = $"{reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
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
                        default:
                            break;
                    }
                }
            }
        }
    }
}
