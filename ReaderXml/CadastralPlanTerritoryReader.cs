using ReaderXml.Fillers;
using ReaderXml.Models;
using System;
using System.Xml;

namespace ReaderXml
{
    /// <summary>
    /// Базовый класс для чтения КПТ. Можно считать это фабрикой <see cref="CadastralPlanTerritory"/>.
    /// </summary>
    public class CadastralPlanTerritoryReader
    {
        public CadastralPlanTerritory Read(string fileName)
        {
            try
            {
                //TODO check file exists
                var result = new CadastralPlanTerritory() { FileName = fileName };
                using (var reader = XmlReader.Create(fileName, new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse }))
                {
                    while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                    {
                    }
                    var filler = FillerFactory.GetFiller(reader.LocalName);
                    if (filler == null)
                    {
                        return null;
                    }
                    filler.Fill(result, reader);
                    reader.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //log
                return null;
            }
        }
    }
}
