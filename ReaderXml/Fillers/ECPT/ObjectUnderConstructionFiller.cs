using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Xml;
using System;

namespace ReaderXml.ECPT
{
    /// <summary>
    /// Объект незавершенного строительства (ОНС).
    /// </summary>
    public class ObjectUnderConstructionFiller : IFiller<Uncompleted>
    {
        public void Fill(Uncompleted model, XmlReader reader)
        {
            try
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
                            case "address_location":
                                {
                                    model.Address += new Address(reader.ReadSubtree()).GetAddress(false);
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
                            case "base_parameter":
                                {
                                    FillBaseParameters(reader.ReadSubtree(), model);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            
            }
            catch (Exception)
            {
                //log
            }
        }

        /// <summary>
        /// Считывает основные характеристики сооружения.
        /// </summary>
        /// <param name="reader">XmlReader узла основных характеристик.</param>
        private void FillBaseParameters(XmlReader reader, Uncompleted model)
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
                                    model.KeyParameters += $"Высота: {reader.ReadElementContentAsString()} м.";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                //log
            }
            
        }
    }
}
