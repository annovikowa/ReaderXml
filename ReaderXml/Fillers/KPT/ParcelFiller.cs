using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Земельный участок.
    /// </summary>
    public class ParcelFiller : IFiller<Parcel>
    {
        public void Fill(Parcel model, XmlReader reader)
        {
            try
            {
                reader.Read();
                #region Присваиваем атрибуты Parcel
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
                                    reader.ReadToDescendant("Area");
                                    model.Area = $"{reader.ReadElementContentAsString()} кв. м.";
                                }
                                break;
                            case "Name":
                                {
                                    if (xsdDictionaries.ParcelsName.TryGetValue(reader.ReadElementContentAsString(), out var name))
                                        model.Name = name;
                                }
                                break;
                            case "Location":
                                {
                                    var inner = reader.ReadSubtree();
                                    model.Address = new Location(inner, xsdDictionaries.AddressRegion).GetAddress(true);
                                    inner.Close();
                                }
                                break;
                            case "Category":
                                {
                                    if (xsdDictionaries.ParcelsCategory.TryGetValue(reader.ReadElementContentAsString(), out var category))
                                        model.Category = category;
                                }
                                break;
                            case "Utilization":
                                {
                                    if (reader.MoveToAttribute("LandUse"))
                                    {
                                        if (xsdDictionaries.LandUse.TryGetValue(reader.Value, out var utilization))
                                            model.Utilization = utilization;
                                    }
                                    if (string.IsNullOrEmpty(model.Utilization))
                                    {
                                        reader.MoveToAttribute("ByDoc");
                                        model.Utilization = reader.Value;
                                    }

                                    if (string.IsNullOrEmpty(model.Utilization))
                                    {
                                        if (xsdDictionaries.Utilization.TryGetValue(reader.Value, out var utilization))
                                            model.Utilization = utilization;
                                    }
                                }
                                break;
                            case "ParentCadastralNumbers":
                                {
                                    reader.ReadToDescendant("CadastralNumber");
                                    model.ParentCadastralNumbers = reader.ReadElementContentAsString();
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
                                {
                                    model.HasCoordinates = true;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
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
