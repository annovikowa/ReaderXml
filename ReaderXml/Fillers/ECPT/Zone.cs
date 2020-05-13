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
    /// Зона.
    /// </summary>
    public class Zone : ICadastralObject
    {
        #region Свойства
        /// <summary>
        /// Учетный номер.
        /// </summary>
        public string RegNumbBorder { get; set; }

        /// <summary>
        /// Вид зоны.
        /// </summary>
        public string TypeBoundary { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string TypeZone { get; set; }

        /// <summary>
        /// Дата постановки на учет.
        /// </summary>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Дополнительная информация.
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Координаты.
        /// </summary>
        public bool isCoordinates { get; set; }
        public string CoorSys { get; set; }
        #endregion

        public void Init(XmlReader reader, XsdClassifiers dictionary = null)
        {
            string NameByDoc = "";
            string TypeZone = "";
            string Number = "";
            string Index = "";
            string WaterObjectName = "";
            string WaterObjectType = "";
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "reg_numb_border":
                            {
                                RegNumbBorder = reader.ReadElementContentAsString();
                            }
                            break;
                        case "registration_date":
                            {
                                RegistrationDate = reader.ReadElementContentAsString();
                            }
                            break;
                        case "type_boundary":
                            {
                                reader.ReadToDescendant("value");
                                TypeBoundary = reader.ReadElementContentAsString();
                            }
                            break;
                        case "type_zone":
                            {
                                reader.ReadToDescendant("value");
                                this.TypeZone = reader.ReadElementContentAsString();
                                TypeZone += $"тип зоны: {this.TypeZone}";
                            }
                            break;                        
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            isCoordinates = true;
                            break;
                        case "name_by_doc":
                            {
                                NameByDoc += $"Наименование по документу: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "number":
                            {
                                Number += $"№{reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "index":
                            {
                                Index += $"Индекс: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "water_object_name":
                            {
                                WaterObjectName += $"Наименование водного объекта: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "water_object_type":
                            {
                                reader.ReadToDescendant("value");
                                WaterObjectType += $"Вид водного объекта: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            var text = new[] { NameByDoc, TypeZone, Number, Index, WaterObjectName, WaterObjectType }.Where(x => !string.IsNullOrEmpty(x));
            AdditionalInformation = string.Join(". ", text);
        }      
    }
}
