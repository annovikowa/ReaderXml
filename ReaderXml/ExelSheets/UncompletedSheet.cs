using ClosedXML.Excel;
using ReaderXml.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReaderXml.ExelSheets
{
    /// <summary>
    /// Лист "ОНС" (Объекты незавершенного строильства).
    /// </summary>
    public class UncompletedSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "ОНС";

        public List<string> Column => new List<string>() { "Кадастровый номер", "Номер кадастрового квартала", "Наличие координат", "Системы координат", "Характеристики", "Назначение", "Адрес", "Кадастровая стоимость" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberUncompleted { get; set; } = 2;

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numUncompleted = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var u in cadastralBlock.Uncompleteds)
                    {
                        sheet.Cell(numObjKpt, "A").Value = u.CadastralNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberUncompleted++}");
                        sheet.Cell(numObjKpt, "B").Value = u.ObjectType;
                        sheet.Cell(numObjKpt++, "C").Value = u.HasCoordinates;
                        numUncompleted++;
                    }
                } while (numUncompleted <= cadastralBlock.Uncompleteds.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var uncompletedsExel = from u in cadastralBlock.Uncompleteds
                                       select new
                                       {
                                           CadastralNumber = u.CadastralNumber,
                                           CadastralNumberKPT = u.CadasstralBlockNumber,
                                           isCoordinates = u.HasCoordinates,
                                           EntSys = u.CoorSys,
                                           KeyParameters = u.KeyParameters,
                                           ObjectType = u.ObjectType,
                                           Address = u.Address,
                                           CadastralCost = u.CadastralCost
                                       };
                Sheet.Cell(_numberUncompleted, 1).Value = uncompletedsExel.AsEnumerable();
            }
        }
    }
}
