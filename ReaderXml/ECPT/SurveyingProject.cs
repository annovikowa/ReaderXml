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
    /// Проект межевания.
    /// </summary>
    public class SurveyingProject : ICadastralObject
    {
        /// <summary>
        /// Учетный номер ПМТ.
        /// </summary>
        public string SurveyProjectNum { get; set; }

        /// <summary>
        /// Условный номер ЗУ.
        /// </summary>
        public string NominalNumber { get; set; }

        /// <summary>
        /// Координаты.
        /// </summary>
        public bool isCoordinates { get; set; }

        public string CoorSys { get; set; }

        public void Init(XmlReader reader, XsdClassifiers dictionary = null)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "survey_project_num":
                            {
                                SurveyProjectNum = reader.ReadElementContentAsString();
                            }
                            break;
                        case "nominal_number":
                            {
                                NominalNumber = reader.ReadElementContentAsString();
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
                        default:
                            break;
                    }
                }
            }
        }
    }
}
