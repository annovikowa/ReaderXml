using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPTv10
{
    /// <summary>
    /// Адрес кадастрового объекта.
    /// </summary>
    public class Address
    {
        #region Свойства
        /// <summary>
        /// Описание адреса.
        /// </summary>
        public Dictionary<string, string> DictionaryAddress { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Лист адресных элементов.
        /// </summary>
        public List<string> SequenceAddress { get; } = new List<string>() { "PostalCode", "Region", "District", "City", "UrbanDistrict" +
            "SovietVillage", "Locality", "Street", "Level1", "Level2", "Level3", "Apartment", "Other" };
        #endregion

        /// <summary>
        /// Инициализация нового экземпляра класса Address.
        /// </summary>
        /// <param name="reader">XmlReader узла адреса.</param>
        /// <param name="dictionaryRegion">Словарь содержащий коды регионов.</param>
        /// <param name="dictionatyAddress">Словарь адресных элементов.</param>
        public Address(XmlReader reader, Dictionary<string, string> dictionaryRegion)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case string node when SequenceAddress.Contains(node):
                            {
                                string value = "";
                                if (reader.MoveToAttribute("Type"))
                                {
                                    value = $"{reader.Value} ";

                                    if (reader.MoveToAttribute("Name"))
                                        value += reader.Value;
                                    else
                                    {
                                        reader.MoveToAttribute("Value");
                                        value += reader.Value;
                                    }
                                    if (!String.IsNullOrWhiteSpace(value))
                                        DictionaryAddress.Add(node, value);
                                }
                                else if (node == "Region")
                                {
                                    if (dictionaryRegion.TryGetValue(reader.ReadElementContentAsString(), out var region))
                                        DictionaryAddress.Add(node, region);
                                }
                                else
                                    DictionaryAddress.Add(node, reader.ReadElementContentAsString());
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Уточнение местоположения и адрес (описание местоположения) кадастрового объекта.
    /// </summary>
    public class Location
    {
        #region Свойства
        /// <summary>
        /// В границах.
        /// </summary>
        public string inBounds { get; set; }

        /// <summary>
        /// Положение ДКК.
        /// </summary>
        public string Placed { get; set; }

        /// <summary>
        /// Уточнение местоположения.
        /// </summary>
        public Elaboration Elaboration { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public Address Address { get; set; }
        #endregion

        /// <summary>
        /// Инициализация нового экземпляра класса Location.
        /// </summary>
        /// <param name="reader">XmlReader узла адреса.</param>
        /// <param name="dictionaryRegion">Словарь содержащий коды регионов.</param>
        /// <param name="dictionaryAddress">Словарь адресных элементов.</param>
        public Location(XmlReader reader, Dictionary<string, string> dictionaryRegion)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "inBounds":
                            {
                                inBounds = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Placed":
                            {
                                Placed = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Elaboration":
                            {
                                var inner = reader.ReadSubtree();
                                Elaboration = new Elaboration(inner);
                                inner.Close();
                            }
                            break;
                        case "Address":
                            {
                                var inner = reader.ReadSubtree();
                                Address = new Address(inner, dictionaryRegion);
                                inner.Close();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Возвращает местоположение кадастрового объекта.
        /// </summary>
        /// <param name="isParcel">Значение "true", если земельный участок; значение "fasle", если другой кадастровый объект.</param>
        /// <returns>Адрес кадастрового объекта.</returns>
        public string GetAddress(bool isParcel)
        {
            string address;
            if (isParcel == true)
            {
                Address.DictionaryAddress.TryGetValue("Note", out var note);
                if (inBounds == "" || inBounds == "2")
                {
                    address = !String.IsNullOrEmpty(note) ? note : OutputAddress();
                }
                else
                {
                    address = inBounds == "1" ? OutputAddress(true) : OutputAddress(false);
                }
            }
            else
                address = OutputAddress();
            return address;

        }
        private string OutputAddress()
        {
            string address = "";
            Address.DictionaryAddress.TryGetValue("Note", out var note);
            if (String.IsNullOrEmpty(note))
            {
                foreach (var s in Address.SequenceAddress)
                {
                    if (Address.DictionaryAddress.TryGetValue(s, out var a))
                        address += $"{a}, ";
                }
            }
            else
                address = note;
            return address;
        }
        private string OutputAddress(bool inBounds)
        {
            if (Elaboration == null)
            {
                Address.DictionaryAddress.TryGetValue("Note", out var note);
                return $"Местоположение установлено относительно ориентира, расположенного {(inBounds == true ? "в границах участка. " : "за пределами участка. ")}" +
                    $"Почтовый адрес ориентира: {(!String.IsNullOrEmpty(note) ? note : OutputAddress())}";
            }
            else
            {
                string landmark = "";
                if (!String.IsNullOrEmpty(Elaboration.Distance) && !String.IsNullOrEmpty(Elaboration.Direction))
                {
                    landmark = $"Участок находится примерно в {Elaboration.Distance} от ориентира по направлению на {Elaboration.Direction}. ";
                }
                Address.DictionaryAddress.TryGetValue("Note", out var note);
                return $"Местоположение установлено относительно ориентира, расположенного {(inBounds == true ? "в границах участка. " : "за пределами участка. ")}" +
                    $"{Elaboration.ReferenceMark}{landmark}" +
                    $"Почтовый адрес ориентира: {(!String.IsNullOrEmpty(note) ? note : OutputAddress())}";
            }
        }
    }

    /// <summary>
    /// Уточнение местоположения.
    /// </summary>
    public class Elaboration
    {
        /// <summary>
        /// Наименование ориентира.
        /// </summary>
        public string ReferenceMark { get; set; }

        /// <summary>
        /// Расстояние.
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Направление.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Инициализация нового экземпляра класса Location.
        /// </summary>
        /// <param name="reader">XmlReader узла уточнения местоположения.</param>
        public Elaboration(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "ReferenceMark":
                            {
                                ReferenceMark = reader.ReadElementContentAsString();
                                ReferenceMark = String.IsNullOrEmpty(ReferenceMark) ? "" : $"Ориентир {ReferenceMark}. ";
                            }
                            break;
                        case "Distance":
                            {
                                Distance = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Direction":
                            {
                                Direction = reader.ReadElementContentAsString();
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
