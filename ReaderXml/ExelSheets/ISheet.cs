using ClosedXML.Excel;
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
        void Transpose();
    }
}
