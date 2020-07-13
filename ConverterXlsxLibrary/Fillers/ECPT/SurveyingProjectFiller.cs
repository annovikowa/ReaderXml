using System.Xml;
using System;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.ECPT
{
    /// <summary>
    /// Проект межевания.
    /// </summary>
    public class SurveyingProjectFiller : IFiller<SurveyingProject>
    {
        public void Fill(SurveyingProject model, XmlReader reader)
        {
            try
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
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }
            
        }
    }
}
