using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Лист "ЗУ" (Земельные участки).
    /// </summary>
    public class ParcelSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "ЗУ";

        public List<string> Column => new List<string>() { "Кадастровый номер", "Номер кадастрового квартала", "Наличие координат", "Системы координат", "Вид ЗУ", "Кадастровый номер ЕЗП (единого землепользования)", "Площадь", "Категория земель", "Виды разрешенного использования", "Адрес", "Кадастровая стоимость" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberParcels { get; set; } = 2;
        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var parcelsExel = from p in cadastralBlock.Parcels
                                  select new
                                  {
                                      CadastralNumber = p.CadastralNumber,
                                      CadastralNumberKPT = p.CadasstralBlockNumber,
                                      isCoordinates = p.HasCoordinates,
                                      EntSys = p.CoorSys,
                                      Name = p.Name,
                                      ParentCadastralNumbers = p.ParentCadastralNumbers,
                                      Area = p.Area,
                                      Category = p.Category,
                                      Utilization = p.Utilization,
                                      Address = p.Address,
                                      CadastralCost = p.CadastralCost
                                  };
                Sheet.Cell(_numberParcels, 1).Value = parcelsExel.AsEnumerable();
            }
        }

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numParcel = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var p in cadastralBlock.Parcels)
                    {
                        sheet.Cell(numObjKpt, "A").Value = p.CadastralNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberParcels++}");
                        sheet.Cell(numObjKpt, "B").Value = p.Category;
                        sheet.Cell(numObjKpt++, "C").Value = p.HasCoordinates;
                        numParcel++;
                    }
                } while (numParcel <= cadastralBlock.Parcels.Count);
            }
        }
    }
}
