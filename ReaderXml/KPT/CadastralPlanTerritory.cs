using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.KPT
{
    public class CadastralPlanTerritory
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public string Official { get; set; }

        public List<CadastralBlock> CadastralBlocks { get; set; } = new List<CadastralBlock>();

        public CadastralPlanTerritory (string fileName)
        {          
            using(var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
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
                                    CadastralBlocks.Add(new CadastralBlock(inner));
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

        
    }
}
