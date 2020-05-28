using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public class ExelFiller
    {
        public XLWorkbook XlWorkbook { get; private set; } = new XLWorkbook();
        private InformationSheet InformationSheet { get; set; }
        private CadastralSheet CadastralSheet { get; set; }
        
        public ExelFiller()
        {
            FillSheet(new InformationSheet());
            FillSheet(new CadastralSheet());
        }

        public void FillSheet<T>(T obj) where T : ISheet, new()
        {
            obj = new T();
            obj.Sheet = XlWorkbook.Worksheets.Add(obj.Title);
            obj.Sheet.Cell(1, 1).Value = obj.Column.AsEnumerable();
            obj.Transpose();
        }
    }
}
