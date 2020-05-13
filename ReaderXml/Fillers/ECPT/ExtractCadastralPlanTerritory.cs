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
    /// Кадастровый план территории.
    /// </summary>
    public class ExtractCadastralPlanTerritory
    {
        #region Свойства
        /// <summary>
        /// Название файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета.
        /// </summary>
        public string OrganRegistrRights { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime DateFormation { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Должностное лицо.
        /// </summary>
        public string Official { get; set; }

        /// <summary>
        /// Сведения о кадастровых объектах.
        /// </summary>
        public List<CadastralBlock> CadastralBlocks { get; set; } = new List<CadastralBlock>();
        #endregion

        /// <summary>
        /// Инициализация нового экземпляра класса ExtractCadastralPlanTerritory.
        /// </summary>
        /// <param name="fileName">URI файла с XML-данными.</param>
        public ExtractCadastralPlanTerritory(string fileName)
        {
            using (var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
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
}
