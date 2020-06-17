using ReaderXml.Fillers;
using ReaderXml.Models;
using System;
using System.Xml;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Граница.
    /// </summary>
    public class BoundFiller : IFiller<Bound>
    {                
        public void Fill(Bound model, XmlReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "AccountNumber":
                                {
                                    model.AccountNumber = reader.ReadElementContentAsString();
                                }
                                break;
                            case "NameNeighbours":
                                {
                                    model.TypeBoundary = "Граница между субъектами Российской Федерации";
                                    if (String.IsNullOrEmpty(model.AdditionalInformation))
                                        model.AdditionalInformation += reader.ReadElementContentAsString();
                                    else
                                        model.AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                                }
                                break;
                            case "MunicipalBoundary":
                                {
                                    model.TypeBoundary = "Граница муниципального образования";
                                    reader.ReadToDescendant("Name");
                                    if (String.IsNullOrEmpty(model.AdditionalInformation))
                                        model.AdditionalInformation += reader.ReadElementContentAsString();
                                    else
                                        model.AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                                }
                                break;
                            case "InhabitedLocalityBoundary":
                                {
                                    model.TypeBoundary = "Граница населенного пункта";
                                    reader.ReadToDescendant("Name");
                                    if (String.IsNullOrEmpty(model.AdditionalInformation))
                                        model.AdditionalInformation += reader.ReadElementContentAsString();
                                    else
                                        model.AdditionalInformation += $", {reader.ReadElementContentAsString()}";
                                }
                                break;
                            case "Description":
                                {
                                    model.Description = reader.ReadElementContentAsString();
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
            catch (Exception)
            {
                //log
            }
            
        }
    }
}
