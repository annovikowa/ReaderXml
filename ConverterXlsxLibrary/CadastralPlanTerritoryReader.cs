using ConverterXlsxLibrary.Fillers;
using ConverterXlsxLibrary.Models;
using System;
using System.Xml;

namespace ConverterXlsxLibrary
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
                    result.FileName = System.IO.Path.GetFileName(fileName);
                    reader.Close();
                    return result;
                }
            }
            catch (Exception)
            {
                //log
                return null;
            }
        }
    }
}
