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
    /// Кадастровый план территории.
    /// </summary>
    public class ExtractCadastralPlanTerritory : Abstract.CadastralPlanTerritory
    {
        /// <summary>
        /// Инициализация нового экземпляра класса ExtractCadastralPlanTerritory.
        /// </summary>
        /// <param name="fileName">URI файла с XML-данными.</param>
        public ExtractCadastralPlanTerritory(string fileName, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "cadastral_block":
                            {
                                var inner = reader.ReadSubtree();
                                CadastralBlocks.Add(new CadastralBlock(inner));
                                inner.Close();
                            }
                            break;
                        case "organ_registr_rights":
                            {
                                OrganRegistrRights = reader.ReadElementContentAsString();
                            }
                            break;
                        case "date_formation":
                            {
                                DateFormation = reader.ReadElementContentAsDateTime();
                            }
                            break;
                        case "registration_number":
                            {
                                RegistrationNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "initials_surname":
                            {
                                Official += $"{reader.ReadElementContentAsString()}, ";
                            }
                            break;
                        case "full_name_position":
                            {
                                Official += $"{reader.ReadElementContentAsString()} ";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
