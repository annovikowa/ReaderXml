using ReaderXml.Fillers;
using ReaderXml.KPT;
using ReaderXml.Models;
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
    public class SurveyingProjectFiller : IFiller<SurveyingProject>
    {
        public void Fill(SurveyingProject model, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "survey_project_num":
                            {
                                model.SurveyProjectNum = reader.ReadElementContentAsString();
                            }
                            break;
                        case "nominal_number":
                            {
                                model.NominalNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "entity_spatial":
                            {
                                reader.ReadToDescendant("sk_id");
                                model.CoorSys = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ordinate":
                            model.HasCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
