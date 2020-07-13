using ConverterXlsxLibrary.Models;
using System.Xml;

namespace ConverterXlsxLibrary.Fillers.KPT
{
    /// <summary>
    /// Здание.
    /// </summary>
    public class BuildingFiller : IFiller<Building>
    {
        public void Fill(Building model, XmlReader reader)
        {
            try
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
                                    model.Address = new Location(inner, xsdDictionaries.AddressRegion, model.Error)?.GetAddress(false);
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
            catch (System.Exception ex)
            {
                model.Error = ex.Message;
            }
            
        }
    }
}
