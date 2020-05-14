﻿using ReaderXml.Fillers;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Здание.
    /// </summary>
    public class BuildingFiller : IFiller<Building>
    {
        public void Fill(Building model, XmlReader reader)
        {
            reader.Read();
            #region Присваиваем атрибуты Building
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        model.CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            var xsdDictionaries = XsdClassifiers.GetInstance();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Area":
                            {
                                model.Area = $"{reader.ReadElementContentAsString()} кв.м.";
                            }
                            break;
                        case "ObjectType":
                            {
                                if (xsdDictionaries.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out var objectType))
                                    model.ObjectType = objectType;
                            }
                            break;
                        case "Address":
                            {
                                var inner = reader.ReadSubtree();
                                model.Address = new Location(inner, xsdDictionaries.AddressRegion)?.GetAddress(false);
                                inner.Close();
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                model.CadastralCost = $"{reader.Value.ToString()} руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                model.CoorSys = reader.Value.ToString();
                            }
                            break;
                        case "Ordinate":
                            model.HasCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}