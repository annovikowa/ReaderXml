using ClosedXML.Excel;
using ReaderXml.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReaderXml.ExelSheets
{
    /// <summary>
    /// Лист "Зоны".
    /// </summary>
    public class ZoneSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Зоны";

        public List<string> Column => new List<string>() { "Учетный номер", "Номер кадастрового квартала", "Вид зоны", "Наименование", "Дата постановки на учет", "Наличие координат", "Системы координат", "Дополнительная информация" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberZone { get; set; } = 2;
        
        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numZone = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var z in cadastralBlock.Zones)
                    {
                        sheet.Cell(numObjKpt, "A").Value = z.AccountNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberZone++}");
                        sheet.Cell(numObjKpt, "B").Value = z.TypeZone;
                        sheet.Cell(numObjKpt++, "C").Value = z.HasCoordinates;
                        numZone++;
                    }
                } while (numZone <= cadastralBlock.Zones.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var zonesExel = from z in cadastralBlock.Zones
                                select new
                                {
                                    AccountNumber = z.AccountNumber,
                                    CadastralNumberKPT = z.CadasstralBlockNumber,
                                    TypeZone = z.TypeZone,
                                    Description = z.Description,
                                    RegistrationDate = z.RegistrationDate,
                                    isCoordinates = z.HasCoordinates,
                                    EntSys = z.CoorSys,
                                    AdditionalInformation = z.AdditionalInformation
                                };
                Sheet.Cell(_numberZone, 1).Value = zonesExel.AsEnumerable();
            }
        }
    }
}
