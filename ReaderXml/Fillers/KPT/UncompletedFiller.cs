using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Объект незавершенного строительства (ОНС).
    /// </summary>
    public class UncompletedFiller : IFiller<Uncompleted>
    {
        public void Fill(Uncompleted model, XmlReader reader)
        {
            try
            {
                reader.Read();
                #region Присваиваем атрибуты Construction
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
                            case "ObjectType":
                                {
                                    if (xsdDictionaries.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out var objectType))
                                        model.ObjectType = objectType;
                                }
                                break;
                            case "Address":
                                {
                                    var inner = reader.ReadSubtree();
                                    model.Address = new Location(inner, xsdDictionaries.AddressRegion).GetAddress(false);
                                    inner.Close();
                                }
                                break;
                            case "CadastralCost":
                                {
                                    reader.MoveToAttribute("Value");
                                    model.CadastralCost = $"{reader.Value} руб.";
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
                            case "KeyParameter":
                                {
                                    reader.MoveToAttribute("Type");
                                    if (xsdDictionaries.KeyParameters.TryGetValue(reader.Value, out var keyParamenter))
                                    {

                                        if (reader.Value == "05" || reader.Value == "06")
                                        {
                                            reader.MoveToAttribute("Value");
                                            model.KeyParameters = $"{keyParamenter}: {reader.Value} кв.м.";
                                        }
                                        else if (reader.Value == "03")
                                        {
                                            reader.MoveToAttribute("Value");
                                            model.KeyParameters = $"{keyParamenter}: {reader.Value} куб.м.";
                                        }
                                        else
                                        {
                                            reader.MoveToAttribute("Value");
                                            model.KeyParameters = $"{keyParamenter}: {reader.Value} м.";
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
            catch (System.Exception)
            {
                //log
            }
            
        }
    }
}
