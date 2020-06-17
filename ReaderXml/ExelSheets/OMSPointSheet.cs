using ClosedXML.Excel;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public class OMSPointSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Пункты ОМС";

        public List<string> Column => new List<string>() { "Номер", "Название и тип", "Класс", "X", "Y", "Номер кадастрового квартала" };

        private int _numberOMSPoint { get; set; } = 2;
        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numOMSPoint = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var o in cadastralBlock.OMSPoints)
                    {
                        sheet.Cell(numObjKpt, "A").Value = o.PNmb;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberOMSPoint++}");
                        sheet.Cell(numObjKpt++, "B").Value = "Пункты ОМС";
                        numOMSPoint++;
                    }
                } while (numOMSPoint <= cadastralBlock.OMSPoints.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var omsExel = from o in cadastralBlock.OMSPoints
                              select new
                              {
                                  PNmb = o.PNmb,
                                  PName = o.PName,
                                  PKlass = o.PKlass,
                                  OrdX = o.OrdX,
                                  OrdY = o.OrdY,
                                  CadastralNumberKPT = o.CadasstralBlockNumber
                              };
                Sheet.Cell(_numberOMSPoint, 1).Value = omsExel.AsEnumerable();
            }
        }
    }
}
