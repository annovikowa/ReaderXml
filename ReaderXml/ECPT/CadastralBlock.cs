using ReaderXml.KPTv10;
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
    public class CadastralBlock : Abstract.CadastralBlock
    {
        /// <summary>
        /// Инициализация нового экземпляра класса CadastralBlock.
        /// </summary>
        /// <param name="reader">XmlReader узла кадастрового квартала.</param>
        public CadastralBlock(XmlReader reader)
        {
            Init(reader);
        }

        public override void Init(XmlReader reader, XsdClassifiers dictionary = null)
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
                                Area = $"{reader.ReadElementContentAsString()} Га";
                            }
                            break;
                        case "cadastral_number":
                            {
                                CadastralNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "land_record":
                            {
                                AddNew(Parcels, reader.ReadSubtree());
                            }
                            break;
                        case "build_record":
                            {
                                AddNew(Buildings, reader.ReadSubtree());
                            }
                            break;
                        case "construction_record":
                            {
                                AddNew(Constructions, reader.ReadSubtree());
                            }
                            break;
                        case "object_under_construction_record":
                            {
                                AddNew(Uncompleteds, reader.ReadSubtree());
                            }
                            break;
                        case "subject_boundary_record":
                        case "municipal_boundary_record":
                        case "inhabited_locality_boundary_record":
                        case "coastline_record":
                            {
                                AddNew(Bounds, reader.ReadSubtree());
                            }
                            break;
                        case "zones_and_territories_record":
                            {
                                AddNew(Zones, reader.ReadSubtree());
                            }
                            break;
                        case "surveying_project_record":
                            {
                                AddNew(SurveyingProjects, reader.ReadSubtree());
                            }
                            break;
                        case "oms_point":
                            {
                                AddNew(OMSPoints, reader.ReadSubtree());
                            }
                            break;
                        case "ordinate":
                            {
                                isCoordinates = true;
                            }
                            break;

                    }
                }
            }
        }
    }
}
