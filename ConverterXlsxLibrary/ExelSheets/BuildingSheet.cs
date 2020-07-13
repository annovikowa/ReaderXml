using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Лист "Здания".
    /// </summary>
    public class BuildingSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Здания";

        public List<string> Column => new List<string>() { "Кадастровый номер", "Номер кадастрового квартала", "Наличие координат", "Системы координат", "Кадастровый номер ЕНК (единого недвижимого комплекса)", "Площадь", "Назначение", "Виды разрешенного использования", "Адрес", "Кадастровая стоимость" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberBuilding { get; set; } = 2;

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numBuilding = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var b in cadastralBlock.Buildings)
                    {
                        sheet.Cell(numObjKpt, "A").Value = b.CadastralNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberBuilding++}");
                        sheet.Cell(numObjKpt, "B").Value = b.ObjectType;
                        sheet.Cell(numObjKpt++, "C").Value = b.HasCoordinates;
                        numBuilding++;
                    }
                } while (numBuilding <= cadastralBlock.Buildings.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var buildingsExel = from b in cadastralBlock.Buildings
                                    select new
                                    {
                                        CadastralNumber = b.CadastralNumber,
                                        CadastralNumberKPT = b.CadasstralBlockNumber,
                                        isCoordinates = b.HasCoordinates,
                                        EntSys = b.CoorSys,
                                        UnitedCadNumbers = b.UnitedCadNumbers,
                                        Area = b.Area,
                                        ObjectType = b.ObjectType,
                                        PermittedUse = b.PermittedUse,
                                        Address = b.Address,
                                        CadastralCost = b.CadastralCost
                                    };

                Sheet.Cell(_numberBuilding, 1).Value = buildingsExel.AsEnumerable();
            }
        }
    }
}
