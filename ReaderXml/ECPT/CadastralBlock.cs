using ReaderXml.KPT;
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
    public class CadastralBlock : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Сведения о земельных участках.
        /// </summary>
        public List<Land> Lands { get; } = new List<Land>();

        /// <summary>
        /// Сведения о зданиях.
        /// </summary>
        public List<Build> Builds { get; } = new List<Build>();

        /// <summary>
        /// Сведения о сооружениях.
        /// </summary>
        public List<Construction> Constructions { get; } = new List<Construction>();

        /// <summary>
        /// Сведения об ОНС.
        /// </summary>
        public List<ObjectUnderConstruction> ObjectUnderConstructions { get; } = new List<ObjectUnderConstruction>();

        /// <summary>
        /// Сведения о границах между субъектами РФ, населенных пунктов, муниципальных образований.
        /// </summary>
        public List<SubjectBoundary> SubjectBoundarys { get; } = new List<SubjectBoundary>();

        /// <summary>
        /// Сведения о зонах.
        /// </summary>
        public List<Zone> Zones { get; } = new List<Zone>();

        /// <summary>
        /// Сведения о проектах межевания.
        /// </summary>
        public List<SurveyingProject> SurveyingProjects { get; } = new List<SurveyingProject>();

        /// <summary>
        /// Сведения о пунктах ОМС.
        /// </summary>
        public List<OMSPoint> OMSPoints { get; } = new List<OMSPoint>();
        #endregion

        /// <summary>
        /// Инициализация нового экземпляра класса CadastralBlock.
        /// </summary>
        /// <param name="reader">XmlReader узла кадастрового квартала.</param>
        public CadastralBlock(XmlReader reader)
        {
            Init(reader);
        }

        private void AddNew<T>(ICollection<T> collection, XmlReader reader) where T : ICadastralObject, new()
        {
            var obj = new T();
            obj.Init(reader, null);
            reader.Close();
            collection.Add(obj);
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
                                AddNew(Lands, reader.ReadSubtree());
                            }
                            break;
                        case "build_record":
                            {
                                AddNew(Builds, reader.ReadSubtree());
                            }
                            break;
                        case "construction_record":
                            {
                                AddNew(Constructions, reader.ReadSubtree());
                            }
                            break;
                        case "object_under_construction_record":
                            {
                                AddNew(ObjectUnderConstructions, reader.ReadSubtree());
                            }
                            break;
                        case "subject_boundary_record":
                            {
                                AddNew(SubjectBoundarys, reader.ReadSubtree());
                            }
                            break;
                        case "municipal_boundary_record":
                            {
                                AddNew(SubjectBoundarys, reader.ReadSubtree());
                            }
                            break;
                        case "inhabited_locality_boundary_record":
                            {
                                AddNew(SubjectBoundarys, reader.ReadSubtree());
                            }
                            break;
                        case "coastline_record":
                            {
                                AddNew(SubjectBoundarys, reader.ReadSubtree());
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
