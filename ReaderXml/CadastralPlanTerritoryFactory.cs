using ReaderXml.Abstract;
using System;
using System.IO;
using System.Xml;

namespace ReaderXml
{
    /// <summary>
    /// Фабрика для создания <see cref="KPTv10.KPT"/> и <see cref="ECPT.ExtractCadastralPlanTerritory"/>
    /// </summary>
    public static class CadastralPlanTerritoryFactory
    {
        public static CadastralPlanTerritory Create(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                {
                    return null;
                }
                using (var reader = XmlReader.Create(fileName, new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse }))
                {
                    while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                    {
                    }
                    switch (reader.LocalName)
                    {
                        case "KPT":
                            {
                                return new KPTv10.KPT(fileName, reader);
                            }
                        case "extract_cadastral_plan_territory":
                            {
                                return new ECPT.ExtractCadastralPlanTerritory(fileName, reader);
                            }
                        default:
                            {
                                return null;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //log exception
                return null;
            }
        }
    }
}
