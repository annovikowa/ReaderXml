using ReaderXml.Fillers;
using ReaderXml.Fillers.ECPT;
using ReaderXml.KPT;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlockFiller : IFiller<CadastralBlock>
    {
        private void AddNew<T>(ICollection<T> collection, XmlReader reader) where T : CadastralObject, new()
        {
            var obj = new T();
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
                                AddNew(model.Parcels, reader.ReadSubtree());
                            }
                            break;
                        case "build_record":
                            {
                                AddNew(model.Buildings, reader.ReadSubtree());
                            }
                            break;
                        case "construction_record":
                            {
                                AddNew(model.Constructions, reader.ReadSubtree());
                            }
                            break;
                        case "object_under_construction_record":
                            {
                                AddNew(model.Uncompleteds, reader.ReadSubtree());
                            }
                            break;
                        case "subject_boundary_record":
                            {
                                AddNew(model.Bounds, reader.ReadSubtree());
                            }
                            break;
                        case "municipal_boundary_record":
                            {
                                AddNew(model.Bounds, reader.ReadSubtree());
                            }
                            break;
                        case "inhabited_locality_boundary_record":
                            {
                                AddNew(model.Bounds, reader.ReadSubtree());
                            }
                            break;
                        case "coastline_record":
                            {
                                AddNew(model.Bounds, reader.ReadSubtree());
                            }
                            break;
                        case "zones_and_territories_record":
                            {
                                AddNew(model.Zones, reader.ReadSubtree());
                            }
                            break;
                        case "surveying_project_record":
                            {
                                AddNew(model.SurveyingProjects, reader.ReadSubtree());
                            }
                            break;
                        case "oms_point":
                            {
                                AddNew(model.OMSPoints, reader.ReadSubtree());
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
    }
}
