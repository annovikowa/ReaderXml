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
    /// Земельный участок.
    /// </summary>
    public class Land : CadastralObject
    {
        #region Свойства
        /// <summary>
        /// Наименование участка.
        /// </summary>
        public string Subtype { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка - Единого землепользования.
        /// </summary>
        public string CommonLandCadNumber { get; set; }

        /// <summary>
        /// Категория земель.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Вид разрешенного использования.
        /// </summary>
        public string PermittedUse { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Сведения о величине кадастровой стоимости.
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        
        public override void Init(XmlReader reader, XsdClassifiers dictionary = null)
        {
            reader.Read();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "common_data":
                            {
                                reader.ReadToDescendant("cad_number");
                                CadastralNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "area":
                            {
                                reader.ReadToDescendant("value");
                                Area = $"{reader.ReadElementContentAsString()} кв. м.";
                            }
                            break;
                        case "subtype":
                            {
                                reader.ReadToDescendant("value");
                                Subtype = reader.ReadElementContentAsString();
                            }
                            break;
                        case "address_location":
                            {
                                Address = new Address(reader.ReadSubtree()).GetAddress(true);
                            }
                            break;
                        case "category":
                            {
                                reader.ReadToDescendant("value");
                                Category = reader.ReadElementContentAsString();
                            }
                            break;
                        case "permitted_use_established":
                            {
                                PermittedUse = FillPermittedUse(reader.ReadSubtree());
                            }
                            break;
                        case "permitted_uses_grad_reg":
                            {
                                if (string.IsNullOrEmpty(PermittedUse))
                                    PermittedUse = FillPermittedUse(reader.ReadSubtree());
                            }
                            break;
                        case "common_land_cad_number":
                            {
                                reader.ReadToDescendant("cad_number");
                                CommonLandCadNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "cost":
                            {
                                reader.ReadToDescendant("value");
                                CadastralCost = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            HasCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает вид разрешенного использования.
        /// </summary>
        /// <param name="reader">XmlReader узла вида разрешенного использования.</param>
        /// <returns>Вид разрешенного использования.</returns>
        private string FillPermittedUse(XmlReader reader)
        {
            string LandUseMer = "";
            string ByDocument = "";
            string LandUse = "";
            string PermittedUseText = "";
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "land_use_mer":
                            {
                                reader.ReadToDescendant("value");
                                LandUseMer = reader.ReadElementContentAsString();
                            }
                            break;
                        case "by_document":
                            {
                                ByDocument = reader.ReadElementContentAsString();
                            }
                            break;
                        case "land_use":
                            {
                                reader.ReadToDescendant("value");
                                LandUse = reader.ReadElementContentAsString();
                            }
                            break;
                        case "permitted_use_text":
                            {
                                PermittedUseText = reader.ReadElementContentAsString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            reader.Close();
            return new[] { LandUseMer, ByDocument, LandUse, PermittedUseText }.FirstOrDefault(x => !string.IsNullOrEmpty(x));
            
        }
    }
}
