using System;
using System.Collections.Generic;
using System.Xml;


namespace ConverterXlsxLibrary.Fillers.ECPT
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
        private Dictionary<string, string> DictionaryAddress { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Ориентир расположен в границах участка.
        /// </summary>
        private string InBoundariesMark { get; set; }

        /// <summary>
        /// Наименование ориентира.
        /// </summary>
        private string RefPointName { get; set; }

        /// <summary>
        /// Расположение относительно ориентира.
        /// </summary>
        private string LocationDescription { get; set; }

        /// <summary>
        /// Адрес в соответствии с ФИАС.
        /// </summary>
        private string ReadableAddress { get; set; }

        /// <summary>
        /// Лист адресных элементов.
        /// </summary>
        private List<string> SequenceAddress { get; } = new List<string>() { "postal_code", "region", "district", "city", "urban_district" +
            "soviet_village", "locality", "street", "level1", "Level2", "Level3", "apartment", "other", "note" };
        #endregion
        
        /// <summary>
        /// Инициализация нового экземпляра класса Address.
        /// </summary>
        /// <param name="reader">XmlReader узла адреса.</param>
        public Address(XmlReader reader, string error)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case string node when SequenceAddress.Contains(node):
                                {
                                    if (node == "postal_code" || node == "other" || node == "note")
                                        DictionaryAddress.Add(node, reader.ReadElementContentAsString());
                                    else if (node == "region")
                                    {
                                        reader.ReadToDescendant("value");
                                        DictionaryAddress.Add(node, reader.ReadElementContentAsString());
                                    }
                                    else
                                        DictionaryAddress.Add(node, ReadNode(reader.ReadSubtree(), node));
                                }
                                break;
                            case "in_boundaries_mark":
                                {
                                    InBoundariesMark = reader.ReadElementContentAsString();
                                }
                                break;
                            case "ref_point_name":
                                {
                                    RefPointName = reader.ReadElementContentAsString();
                                }
                                break;
                            case "location_description":
                                {
                                    LocationDescription = reader.ReadElementContentAsString();
                                }
                                break;
                            case "readable_address":
                                {
                                    ReadableAddress = reader.ReadElementContentAsString();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                error += $" {ex.Message}";
            }
            
        }
        
        /// <summary>
        /// Возвращает местоположение кадастрового объекта.
        /// </summary>
        /// <param name="isLand">Значение "true", если земельный участок; значение "fasle", если другой кадастровый объект.</param>
        /// <returns>Адрес кадастрового объекта.</returns>
        public string GetAddress(bool isLand)
        {
            string address = "";
            if (isLand == true)
            {
                DictionaryAddress.TryGetValue("note", out var note);
                if (InBoundariesMark == "1" || InBoundariesMark == "true")
                {
                    address = OutputAddress(true);
                }
                else
                {
                    address = !String.IsNullOrEmpty(note) ? note : OutputAddress();
                }
            }
            else
                address = OutputAddress();
            return address;
        }

        /// <summary>
        /// Считывает детальный узел местоположения.
        /// </summary>
        /// <param name="reader">XmlReader детального узла.</param>
        /// <param name="node">Имя узла.</param>
        /// <returns>Строку детального местоположения.</returns>
        private string ReadNode(XmlReader reader, string node)
        {
            string str = "";
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == $"type_{node}")
                        str += $"{reader.ReadElementContentAsString()}. ";
                    else if (reader.LocalName == $"name_{node}")
                        str += reader.ReadElementContentAsString();
                    else if (reader.LocalName == $"name__{node}")
                        str += reader.ReadElementContentAsString();
                }
            }
            reader.Close();
            return str;
        }
        private string OutputAddress()
        {
            string address = null;
            DictionaryAddress.TryGetValue("note", out var note);
            if (String.IsNullOrEmpty(note))
            {
                foreach (var s in SequenceAddress)
                {
                    if (DictionaryAddress.TryGetValue(s, out var a))
                        address += $" {a},";
                }
            }
            else
                address = note;
            
            return String.IsNullOrEmpty(address) ? ReadableAddress : address;
        }
        private string OutputAddress(bool InBoundariesMark)
        {
            string Landmark = String.IsNullOrEmpty(RefPointName) ? "" : $"Ориентир {RefPointName}. ";
            string Location = String.IsNullOrEmpty(LocationDescription) ? "" : $"Участок находится примерно в {LocationDescription}. ";
            return $"Местоположение установлено относительно ориентира, расположенного {(this.InBoundariesMark == "1" || this.InBoundariesMark=="true" ? "в границах" : "за пределами")} участка. {Landmark + Location + OutputAddress()}";
        }
    }
}
