using ReaderXml.Fillers;
using ReaderXml.Fillers.ECPT;
using ReaderXml.Models;
using System.Collections.Generic;
using System.Xml;
using System;

namespace ReaderXml.ECPT
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
                                }
                                break;
                            case "build_record":
                                {
                                    AddNew(model.Buildings, reader.ReadSubtree(), model);
                                }
                                break;
                            case "construction_record":
                                {
                                    AddNew(model.Constructions, reader.ReadSubtree(), model);
                                }
                                break;
                            case "object_under_construction_record":
                                {
                                    AddNew(model.Uncompleteds, reader.ReadSubtree(), model);
                                }
                                break;
                            case "subject_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                }
                                break;
                            case "municipal_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                }
                                break;
                            case "inhabited_locality_boundary_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                }
                                break;
                            case "coastline_record":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                }
                                break;
                            case "zones_and_territories_record":
                                {
                                    AddNew(model.Zones, reader.ReadSubtree(), model);
                                }
                                break;
                            case "surveying_project_record":
                                {
                                    AddNew(model.SurveyingProjects, reader.ReadSubtree(), model);
                                }
                                break;
                            case "oms_point":
                                {
                                    AddNew(model.OMSPoints, reader.ReadSubtree(), model);
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
            catch (Exception)
            {
                //log
            }
            
        }
    }
}
