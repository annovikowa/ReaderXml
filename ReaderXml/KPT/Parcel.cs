using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class Parcel : CadastralObject, ICadastralObject
    {
        #region Свойства

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Наименование участка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка - Единого землепользования
        /// </summary>
        public string ParentCadastralNumbers { get; set; }

        /// <summary>
        /// Категория земель
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Вид разрешенного использования
        /// </summary>
        public string Utilization { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Сведения о величине кадастровой стоимости
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        public Parcel()
        {
        }

        public void Init(XmlReader reader, XsdClassifiers dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Parcel
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
                                reader.MoveToContent();
                                reader.ReadToDescendant("Area");
                                Area = $"{reader.ReadElementContentAsString()} кв. м.";
                            }
                            break;
                        case "Name":
                            {
                                if (dictionary.ParcelsName.TryGetValue(reader.ReadElementContentAsString(), out var name))
                                    Name = name;
                            }
                            break;
                        case "Location":
                            {
                                var inner = reader.ReadSubtree();
                                Address = new Location(inner, dictionary.AddressRegion, dictionary.AddressOut).GetAddress(true);
                                inner.Close();
                            }
                            break;
                        case "Category":
                            {
                                if (dictionary.ParcelsCategory.TryGetValue(reader.ReadElementContentAsString(), out var category))
                                    Category = category;
                            }
                            break;
                        case "Utilization":
                            {
                                if (reader.MoveToAttribute("LandUse"))
                                {
                                    if (dictionary.LandUse.TryGetValue(reader.Value, out var utilization))
                                        Utilization = utilization;
                                }
                                if (String.IsNullOrEmpty(Utilization))
                                {
                                    reader.MoveToAttribute("ByDoc");
                                    Utilization = reader.Value;
                                }

                                if (String.IsNullOrEmpty(Utilization))
                                {
                                    if (dictionary.Utilization.TryGetValue(reader.Value, out var utilization))
                                        Utilization = utilization;
                                }
                            }
                            break;
                        case "ParentCadastralNumbers":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("CadastralNumber");
                                ParentCadastralNumbers = reader.ReadElementContentAsString();
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
