﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class Uncompleted : ICadastralObject
    {
        #region
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Основные характеристики ОНС
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion

        public Uncompleted()
        {
        }

        public void Init(XmlReader reader, Dictionary dictionary)
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
                                Address = new Location(inner, dictionary.AddressRegion, dictionary.AddressOut).GetAddress(false);
                                inner.Close();
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = $"{reader.Value} руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                if (dictionary.CoordSystems.TryGetValue(reader.Value.ToString(), out var entSys))
                                    EntSys = entSys;
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
