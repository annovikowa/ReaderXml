using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReaderXml.KPT
{
    public class CadastralBlock : CadastralObject
    {
        #region
        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Сведения о земельных участках
        /// </summary>
        public List<Parcel> Parcels { get; } = new List<Parcel>();

        /// <summary>
        /// Здания
        /// </summary>
        public List<Building> Buildings { get; } = new List<Building>();

        /// <summary>
        /// Сооружение
        /// </summary>
        public List<Construction> Constructions { get; } = new List<Construction>();

        /// <summary>
        /// ОНС
        /// </summary>
        public List<Uncompleted> Uncompleteds { get; } = new List<Uncompleted>();

        /// <summary>
        /// Границы между субъектами РФ, границы населенных пунктов, муниципальных образований, расположенных в кадастровом квартале
        /// </summary>
        public List<Bound> Bounds { get; } = new List<Bound>();

        /// <summary>
        /// Зоны
        /// </summary>
        public List<Zone> Zones { get; } = new List<Zone>();

        /// <summary>
        /// Сведения о пунктах ОМС
        /// </summary>
        public List<OMSPoint> OMSPoints { get; } = new List<OMSPoint>();        

        private Dictionary<string, string> CoordSystem { get; set; }
        #endregion


        public CadastralBlock(XmlReader reader, XsdClassifiers dictionary)
        {            
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Total":
                            {
                                Area = $"{reader.ReadElementContentAsString()} Га";
                            }
                            break;
                        case "CadastralBlock":
                            {
                                reader.MoveToAttribute("CadastralNumber");
                                CadastralNumber = reader.Value;
                            }
                            break;
                        case "Parcel":
                            {
                                AddNew(Parcels, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Building":
                            {
                                AddNew(Buildings, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Construction":
                            {
                                AddNew(Constructions, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Uncompleted":
                            {
                                AddNew(Uncompleteds, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Bound":
                            {
                                AddNew(Bounds, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Zone":
                            {
                                AddNew(Zones, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "OMSPoint":
                            {
                                AddNew(OMSPoints, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "CoordSystems":
                            {
                                this.CoordSystem = FillCoordSystems(reader.ReadSubtree());
                            }
                            break;
                        case "Ordinate":
                            {
                                isCoordinates = true;
                            }
                            break;

                    }
                }
            }
            var allObjects = Parcels.Union<ICadastralObject>(Buildings).Union(Constructions).Union(Uncompleteds).Union(Bounds).Union(Zones);
            DefiningCoordinateSystem(allObjects);
        }

        private Dictionary<string, string> FillCoordSystems(XmlReader reader)
        {
            var dictionary = new Dictionary<string, string>();
            reader.ReadToDescendant("CoordSystems");
            while (reader.Read())
            {
                if (reader.LocalName == "CoordSystem")
                {
                    reader.MoveToAttribute("CsId").ToString();
                    string CsId = reader.Value;
                    reader.MoveToAttribute("Name");
                    dictionary.Add(CsId, reader.Value);
                }
            }
            return dictionary;
        }
        private void AddNew<T>(ICollection<T> collection, XmlReader reader, XsdClassifiers dictionary) where T : ICadastralObject, new()
        {
            var obj = new T();
            obj.Init(reader, dictionary);
            reader.Close();
            collection.Add(obj);
        }
        private void DefiningCoordinateSystem(IEnumerable<ICadastralObject> objects)
        {
            foreach (var p in objects)
            {
                if (!String.IsNullOrEmpty(p.EntSys))
                {
                    if (CoordSystem.TryGetValue(p.EntSys, out var entSys))
                        p.EntSys = entSys;
                }
            }
        }
    }
}
