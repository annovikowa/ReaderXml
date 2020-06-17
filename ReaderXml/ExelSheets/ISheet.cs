using ClosedXML.Excel;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public interface ISheet
    {
        IXLWorksheet Sheet { get; set; }
        string Title { get; }
        List<string> Column { get; }
        void Fill(CadastralPlanTerritory KPT);
        void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt);
    }
}
