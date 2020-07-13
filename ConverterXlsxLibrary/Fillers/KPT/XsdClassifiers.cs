using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ConverterXlsxLibrary.Fillers.KPT
{
    public class XsdClassifiers
    {
        private static object _syncObject = new object();

        private static XsdClassifiers _instance;

        private List<string> _listDictionary { get; } = new List<string>() { "dRegionsRF_v01", "dParcels_v01", "dCategories_v01", "dAllowedUse_v02",
            "dUtilizations_v01", "dRealty_v03", "dTypeParameter_v01", "dPermitUse_v01", "_AddressOut_v04" };

        public Dictionary<string, string> AddressRegion { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsName { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsCategory { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Utilization { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LandUse { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ObjectType { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> KeyParameters { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> PermitUse { get; private set; } = new Dictionary<string, string>();

        private XsdClassifiers()
        {
            FillXsdClassifiers();
        }

        public static XsdClassifiers GetInstance()
        {
            lock(_syncObject)
            {
                if (_instance == null)
                {
                    _instance = new XsdClassifiers();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Заполнение нового экземпляра XsdClassifiers.
        /// </summary>
        /// <returns>Новый экземпляр XsdClassifiers.</returns>
        private void FillXsdClassifiers()
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            try
            {
                foreach (var list in _listDictionary)
                {
                    var xsd = XElement.Load($@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\{list}.xsd");
                    switch (list)
                    {
                        case "dRegionsRF_v01":
                            {
                                AddressRegion = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dParcels_v01":
                            {
                                ParcelsName = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dCategories_v01":
                            {
                                ParcelsCategory = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dUtilizations_v01":
                            {
                                Utilization = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dAllowedUse_v02":
                            {
                                LandUse = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dRealty_v03":
                            {
                                ObjectType = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dTypeParameter_v01":
                            {
                                KeyParameters = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        case "dPermitUse_v01":
                            {
                                PermitUse = FillXsdClassifiers(xsd, nsManager);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                //log
            }
            
        }
        private Dictionary<string, string> FillXsdClassifiers(XElement xsd, XmlNamespaceManager nsManager)
        {
            return xsd.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());
        }
    }
}
