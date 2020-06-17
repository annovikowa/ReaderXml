using ClosedXML.Excel;
using ReaderXml.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReaderXml.ExelSheets
{
    /// <summary>
    /// Лист "Сооружения".
    /// </summary>
    public class ConstructionSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Сооружения";

        public List<string> Column => new List<string>() { "Кадастровый номер", "Номер кадастрового квартала", "Наличие координат", "Системы координат", "Кадастровый номер ЕНК (единого недвижимого комплекса)", "Характеристики", "Назначение", "Виды разрешенного использования", "Адрес", "Кадастровая стоимость" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberConstruction { get; set; } = 2;

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numConstruction = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var c in cadastralBlock.Constructions)
                    {
                        sheet.Cell(numObjKpt, "A").Value = c.CadastralNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberConstruction++}");
                        sheet.Cell(numObjKpt, "B").Value = c.ObjectType;
                        sheet.Cell(numObjKpt++, "C").Value = c.HasCoordinates;
                        numConstruction++;
                    }
                } while (numConstruction <= cadastralBlock.Constructions.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var constructionsExel = from c in cadastralBlock.Constructions
                                        select new
                                        {
                                            CadastralNumber = c.CadastralNumber,
                                            CadastralNumberKPT = c.CadasstralBlockNumber,
                                            isCoordinates = c.HasCoordinates,
                                            EntSys = c.CoorSys,
                                            UnitedCadNumbers = c.UnitedCadNumbers,
                                            KeyParameters = c.KeyParameters,
                                            ObjectType = c.ObjectType,
                                            PermittedUse = c.PermittedUse,
                                            Address = c.Address,
                                            CadastralCost = c.CadastralCost
                                        };
                Sheet.Cell(_numberConstruction, 1).Value = constructionsExel.AsEnumerable();
            }
        }
    }
}
