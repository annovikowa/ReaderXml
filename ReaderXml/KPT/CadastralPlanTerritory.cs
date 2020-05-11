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
    /// <summary>
    /// Кадастровый план территории.
    /// </summary>
    public class CadastralPlanTerritory
    {
        #region Свойства
        /// <summary>
        /// Название файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Должностное лицо.
        /// </summary>
        public string Official { get; set; }

        /// <summary>
        /// Сведения о кадастровых объектах.
        /// </summary>
        public List<CadastralBlock> CadastralBlocks { get; set; } = new List<CadastralBlock>();

        private List<string> ListDictionary { get; } = new List<string>() { "dRegionsRF_v01", "dParcels_v01", "dCategories_v01", "dAllowedUse_v02",
            "dUtilizations_v01", "dRealty_v03", "dTypeParameter_v01", "dPermitUse_v01", "_AddressOut_v04" };
        #endregion

        /// <summary>
        /// Инициализация нового экземпляра класса CadastralPlanTerritory.
        /// </summary>
        /// <param name="fileName">URI файла с XML-данными.</param>
        public CadastralPlanTerritory (string fileName)
        {            
            using (var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "CadastralBlock":
                                {
                                    var inner = reader.ReadSubtree();
                                    CadastralBlocks.Add(new CadastralBlock(inner, FillXsdClassifiers()));
                                    inner.Close();
                                }
                                break;
                            case "Organization":
                                {
                                    Organization = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Date":
                                {
                                    Date = reader.ReadElementContentAsDateTime();
                                }
                                break;
                            case "Number":
                                {
                                    Number = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Appointment":
                                {
                                    Official += $"{reader.ReadElementContentAsString()}, ";
                                }
                                break;
                            case "FamilyName":
                                {
                                    Official += $"{reader.ReadElementContentAsString()} ";
                                }
                                break;
                            case "FirstName":
                                {
                                    Official += $"{reader.ReadElementContentAsString()} ";
                                }
                                break;
                            case "Patronymic":
                                {
                                    Official += $"{reader.ReadElementContentAsString()}";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Заполнение нового экземпляра XsdClassifiers.
        /// </summary>
        /// <returns>Новый экземпляр XsdClassifiers.</returns>
        private XsdClassifiers FillXsdClassifiers()
        {
            XsdClassifiers dictionary = new XsdClassifiers();
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            foreach (var list in ListDictionary)
            {
                var xsd = XElement.Load(@$"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\{list}.xsd");
                switch (list)
                {
                    case "dRegionsRF_v01":
                        {
                            dictionary.AddressRegion = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dParcels_v01":
                        {
                            dictionary.ParcelsName = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dCategories_v01":
                        {
                            dictionary.ParcelsCategory = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dUtilizations_v01":
                        {
                            dictionary.Utilization = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dAllowedUse_v02":
                        {
                            dictionary.LandUse = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dRealty_v03":
                        {
                            dictionary.ObjectType = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dTypeParameter_v01":
                        {
                            dictionary.KeyParameters = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    case "dPermitUse_v01":
                        {
                            dictionary.PermitUse = FillXsdClassifiers(xsd, nsManager);
                        }
                        break;
                    default:
                        break;
                }
            }
            return dictionary;
        }
        private Dictionary<string, string> FillXsdClassifiers(XElement xsd, XmlNamespaceManager nsManager)
        {
            return xsd.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());
        }
    }
}
