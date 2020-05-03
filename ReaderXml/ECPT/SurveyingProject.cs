using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.ECPT
{
    public class SurveyingProject : ICadastralObject
    {
        /// <summary>
        /// Учетный номер ПМТ
        /// </summary>
        public string SurveyProjectNum { get; set; }

        /// <summary>
        /// Условный номер ЗУ
        /// </summary>
        public string NominalNumber { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        public string SkId { get; set; }

        public void Init(XmlReader reader)
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
                                reader.MoveToContent();
                                reader.ReadToDescendant("sk_id");
                                SkId = reader.ReadElementContentAsString();
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
