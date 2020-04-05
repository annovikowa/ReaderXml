using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class Address
    {
        #region
        public Dictionary<string, string> DictionaryAddress { get; set; } = new Dictionary<string, string>();

        public List<string> SequenceAddress { get; } = new List<string>() { "PostalCode", "Region", "District", "City", "UrbanDistrict" +
            "SovietVillage", "Locality", "Street", "Level1", "Level2", "Level3", "Apartment", "Other" };
        
        #endregion

        public Address(XmlReader reader, Dictionary<string, string> dictionaryRegion, Dictionary<string, string> dictionatyAddress)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case string node when dictionatyAddress.TryGetValue(node, out var value):
                            {
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
    public class Location
    {
        /// <summary>
        /// В границах
        /// </summary>
        public string inBounds { get; set; }

        /// <summary>
        /// Положение ДКК
        /// </summary>
        public string Placed { get; set; }

        /// <summary>
        /// Уточнение местоположения
        /// </summary>
        public Elaboration Elaboration { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public Address Address { get; set; }

        public Location(XmlReader reader, Dictionary<string, string> dictionaryRegion, Dictionary<string, string> dictionaryAddress)
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
                                Address = new Address(inner, dictionaryRegion, dictionaryAddress);
                                inner.Close();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

        }

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
            foreach (var s in Address.SequenceAddress)
            {
                if (Address.DictionaryAddress.TryGetValue(s, out var a))
                    address += $"{a}, ";
            }
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
    public class Elaboration
    {
        /// <summary>
        /// Наименование ориентира
        /// </summary>
        public string ReferenceMark { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        public string Direction { get; set; }

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
