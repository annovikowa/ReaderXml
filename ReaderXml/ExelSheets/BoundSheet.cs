using ClosedXML.Excel;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public class BoundSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Границы";

        public List<string> Column => new List<string>() { "Учетный номер", "Номер кадастрового квартала", "Вид границы", "Наименование", "Дата постановки на учет", "Наличие координат", "Системы координат", "Дополнительная информация" };

        private int _numberBound { get; set; } = 2;
        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numBound = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var b in cadastralBlock.Bounds)
                    {
                        sheet.Cell(numObjKpt, "A").Value = b.AccountNumber;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberBound++}");
                        sheet.Cell(numObjKpt, "B").Value = b.TypeBoundary;
                        sheet.Cell(numObjKpt++, "C").Value = b.HasCoordinates;
                        numBound++;
                    }
                } while (numBound <= cadastralBlock.Bounds.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var boundsExel = from b in cadastralBlock.Bounds
                                 select new
                                 {
                                     AccountNumber = b.AccountNumber,
                                     CadastralNumberKPT = b.CadasstralBlockNumber,
                                     TypeBoundary = b.TypeBoundary,
                                     Description = b.Description,
                                     RegistrationDate = b.RegistrationDate,
                                     isCoordinates = b.HasCoordinates,
                                     EntSys = b.CoorSys,
                                     AdditionalInformation = b.AdditionalInformation
                                 };
                Sheet.Cell(_numberBound, 1).Value = boundsExel.AsEnumerable();
            }
        }
    }
}
