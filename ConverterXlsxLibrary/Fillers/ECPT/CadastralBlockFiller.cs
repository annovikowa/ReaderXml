using System.Collections.Generic;
using System.Xml;
using System;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.ECPT
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlockFiller : IFiller<CadastralBlock>
    {
        private void AddNew<T>(ICollection<T> collection, XmlReader reader, CadastralBlock model) where T : CadastralObjectInBlock, new()
        {
            var obj = new T();
            obj.SetCadastralBlock(model);
            var filler = ECPTFillerFactory.GetFiller(obj);
            if (filler == null)
            {
                return;
            }
            filler.Fill(obj, reader);
            reader.Close();
            collection.Add(obj);
        }

        public void Fill(CadastralBlock model, XmlReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "area_quarter":
                                {
                                    reader.ReadToDescendant("area");
                                    model.Area = $"{reader.ReadElementContentAsString()} Га";
                                }
                                break;
                            case "cadastral_number":
                                {
                                    model.CadastralNumber = reader.ReadElementContentAsString();
                                }
                                break;
                            case "land_record":
                                {
                                    AddNew(model.Parcels, reader.ReadSubtree(), model);
                                    foreach (var er in model.Parcels)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "build_record":
                                {
                                    AddNew(model.Buildings, reader.ReadSubtree(), model);
                                    foreach (var er in model.Buildings)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "construction_record":
                                {
                                    AddNew(model.Constructions, reader.ReadSubtree(), model);
                                    foreach (var er in model.Constructions)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "object_under_construction_record":
                                {
                                    AddNew(model.Uncompleteds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Uncompleteds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "subject_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Bounds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "municipal_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Bounds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "inhabited_locality_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Bounds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "coastline_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Bounds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "zones_and_territories_record":
                                {
                                    AddNew(model.Zones, reader.ReadSubtree(), model);
                                    foreach (var er in model.Zones)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "surveying_project_record":
                                {
                                    AddNew(model.SurveyingProjects, reader.ReadSubtree(), model);
                                    foreach (var er in model.SurveyingProjects)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "oms_point":
                                {
                                    AddNew(model.OMSPoints, reader.ReadSubtree(), model);
                                    foreach (var er in model.OMSPoints)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "ordinate":
                                {
                                    model.HasCoordinates = true;
                                }
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.Errors.Add(ex.Message);
            }
            
        }
    }
}
