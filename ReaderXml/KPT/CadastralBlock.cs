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

        private List<string> ListDictionary { get; } = new List<string>() { "dRegionsRF_v01", "dParcels_v01", "dCategories_v01", "dAllowedUse_v02",
            "dUtilizations_v01", "dRealty_v03", "dTypeParameter_v01", "dPermitUse_v01", "_AddressOut_v04" };

        private Dictionary<string, string> CoordSystem { get; set; }
        #endregion
        public CadastralBlock(XmlReader reader)
        {
            var dictionary = FillDictionary();
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
                                Parcels.Add(AddNew(new Parcel(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "Building":
                            {
                                Buildings.Add(AddNew(new Building(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "Construction":
                            {
                                Constructions.Add(AddNew(new Construction(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "Uncompleted":
                            {
                                Uncompleteds.Add(AddNew(new Uncompleted(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "Bound":
                            {
                                Bounds.Add(AddNew(new Bound(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "Zone":
                            {
                                Zones.Add(AddNew(new Zone(), reader.ReadSubtree(), dictionary));
                            }
                            break;
                        case "OMSPoint":
                            {
                                OMSPoints.Add(AddNew(new OMSPoint(), reader.ReadSubtree(), null));
                            }
                            break;
                        case "CoordSystems":
                            {
                                this.CoordSystem = FillDictionary(reader.ReadSubtree());
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
            DefiningCoordinateSystem(Parcels);
            DefiningCoordinateSystem(Buildings);
            DefiningCoordinateSystem(Constructions);
            DefiningCoordinateSystem(Uncompleteds);
            DefiningCoordinateSystem(Bounds);
            DefiningCoordinateSystem(Zones);
        }

        private Dictionary FillDictionary()
        {
            Dictionary dictionary = new Dictionary();
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            foreach (var list in ListDictionary)
            {
                var xsd = XElement.Load(@$"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\{list}.xsd");
                switch (list)
                {
                    case "dRegionsRF_v01":
                        {
                            dictionary.AddressRegion = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dParcels_v01":
                        {
                            dictionary.ParcelsName = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dCategories_v01":
                        {
                            dictionary.ParcelsCategory = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dUtilizations_v01":
                        {
                            dictionary.Utilization = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dAllowedUse_v02":
                        {
                            dictionary.LandUse = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dRealty_v03":
                        {
                            dictionary.ObjectType = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dTypeParameter_v01":
                        {
                            dictionary.KeyParameters = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "dPermitUse_v01":
                        {
                            dictionary.PermitUse = FillDictionary(xsd, nsManager);
                        }
                        break;
                    case "_AddressOut_v04":
                        {
                            dictionary.AddressOut = xsd.XPathSelectElements(".//xs:element", nsManager)
                                 .ToDictionary(x => x.XPathEvaluate("string(@name)").ToString(),
                                 x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            return dictionary;
        }
        private Dictionary<string, string> FillDictionary(XmlReader reader)
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
        private Dictionary<string, string> FillDictionary(XElement xsd, XmlNamespaceManager nsManager)
        {
            return xsd.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());
        }
        private T AddNew<T>(T obj, XmlReader reader, Dictionary dictionary) where T : ICadastralObject, new()
        {
            obj = new T();
            obj.Init(reader, dictionary);
            reader.Close();
            return obj;
        }
        private void DefiningCoordinateSystem<T>(List<T> obj) where T : ICadastralObject
        {
            foreach (var p in obj)
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
