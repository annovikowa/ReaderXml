using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Лист "Кадастровые кварталы".
    /// </summary>
    public class CadastralSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title { get; } = "Кадастровые кварталы";

        public List<string> Column { get; } = new List<string>() { "Номер кадастрового квартала", "Наличие координат", "Площадь" };

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            int line = 2;
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                for (;;)
                {
                    int column = 1;
                    Sheet.Cell(line, column++).Value = cadastralBlock.CadastralNumber;
                    Sheet.Cell(line, column++).Value = cadastralBlock.HasCoordinates;
                    Sheet.Cell(line++, column).Value = cadastralBlock.Area;
                    break;
                }
            }
        }
    }
}
