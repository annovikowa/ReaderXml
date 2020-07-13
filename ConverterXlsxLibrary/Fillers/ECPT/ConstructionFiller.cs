﻿using System.Xml;
using System;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.ECPT
{
    /// <summary>
    /// Сооружение.
    /// </summary>
    public class ConstructionFiller : IFiller<Construction>
    {
        public void Fill(Models.Construction model, XmlReader reader)
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
                                model.CadastralNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "purpose":
                            {
                                model.ObjectType = reader.ReadElementContentAsString();
                            }
                            break;
                        case "base_parameter":
                            {
                                FillBaseParameters(reader.ReadSubtree(), model);
                            }
                            break;
                        case "address":
                            {
                                model.Address += $"{new Address(reader.ReadSubtree(), model.Error).GetAddress(false)}; ";
                            }
                            break;
                        case "location":
                            {
                                model.Address += $"{new Address(reader.ReadSubtree(), model.Error).GetAddress(false)}; ";
                            }
                            break;
                        case "permitted_use":
                            {
                                reader.ReadToDescendant("name");
                                model.PermittedUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "united_cad_number":
                            {
                                reader.ReadToDescendant("cad_number");
                                model.UnitedCadNumbers = reader.ReadElementContentAsString();
                            }
                            break;
                        case "cost":
                            {
                                reader.ReadToDescendant("value");
                                model.CadastralCost = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                model.CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            model.HasCoordinates = true;
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
        private void FillBaseParameters(XmlReader reader, Construction model)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "area":
                                {
                                    model.KeyParameters += $"Площадь: {reader.ReadElementContentAsString()} кв.м. ";
                                }
                                break;
                            case "built_up_area":
                                {
                                    model.KeyParameters += $"Площадь: {reader.ReadElementContentAsString()} кв.м. ";
                                }
                                break;
                            case "extension":
                                {
                                    model.KeyParameters += $"Протяженность: {reader.ReadElementContentAsString()} м. ";
                                }
                                break;
                            case "depth":
                                {
                                    model.KeyParameters += $"Глубина: {reader.ReadElementContentAsString()} м. ";
                                }
                                break;
                            case "occurence_depth":
                                {
                                    model.KeyParameters += $"Глубина залегания: {reader.ReadElementContentAsString()} м. ";
                                }
                                break;
                            case "volume":
                                {
                                    model.KeyParameters += $"Объем: {reader.ReadElementContentAsString()} куб.м. ";
                                }
                                break;
                            case "height":
                                {
                                    model.KeyParameters += $"Высота: {reader.ReadElementContentAsString()} м. ";
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
                model.Error = ex.Message;
            }
           
        }
    }
}
