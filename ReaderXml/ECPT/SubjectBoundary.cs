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
    /// Граница.
    /// </summary>
    public class SubjectBoundary : ICadastralObject
    {
        #region Свойства
        /// <summary>
        /// Учетный номер.
        /// </summary>
        public string RegNumbBorder { get; set; }

        /// <summary>
        /// Вид границы.
        /// </summary>
        public string TypeBoundary { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата постановки на учет.
        /// </summary>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Координаты.
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Дополнительная информация.
        /// </summary>
        public string AdditionalInformation { get; set; }
        public string CoorSys { get; set; }
        #endregion
        
        public void Init(XmlReader reader, XsdClassifiers dictionary = null)
        {
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
                        case "description":
                            {
                                Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "sk_id":
                            {
                                CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "neighbour_regions":
                            {
                                AdditionalInformation = $"Смежные субъекты: {reader.ReadElementContentAsString()}";
                            }
                            break;
                        case "ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
