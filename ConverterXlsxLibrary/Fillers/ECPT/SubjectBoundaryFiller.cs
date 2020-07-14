﻿using System.Xml;
using System;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.ECPT
{
    /// <summary>
    /// Граница.
    /// </summary>
    public class SubjectBoundaryFiller : IFiller<Bound>
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
                            case "reg_numb_border":
                                {
                                    model.AccountNumber = reader.ReadElementContentAsString();
                                }
                                break;
                            case "registration_date":
                                {
                                    model.RegistrationDate = reader.ReadElementContentAsString();
                                }
                                break;
                            case "type_boundary":
                                {
                                    reader.ReadToDescendant("value");
                                    model.TypeBoundary = reader.ReadElementContentAsString();
                                }
                                break;
                            case "description":
                                {
                                    model.Description = reader.ReadElementContentAsString();
                                }
                                break;
                            case "sk_id":
                                {
                                    model.CoorSys = reader.ReadElementContentAsString();
                                }
                                break;
                            case "neighbour_regions":
                                {
                                    model.AdditionalInformation = $"Смежные субъекты: {reader.ReadElementContentAsString()}";
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
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }
           
        }
    }
}