using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public class CadastralSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title { get; } = "Кадастровые кварталы";

        public List<string> Column { get; } = new List<string>() { "Номер кадастрового квартала", "Наличие координат", "Площадь" };

        public void Transpose()
        {
            var rng = Sheet.Range("A1:A3");
            rng.Transpose(XLTransposeOptions.MoveCells);
        }
    }
}
