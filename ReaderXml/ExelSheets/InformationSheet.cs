using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.ExelSheets
{
    public class InformationSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title { get; } = "Сводная информация";

        public List<string> Column { get; } = new List<string>() { "Имя XML-файла", "Наименование органа регистрации прав", "Номер и дата выдачи КПТ", "ФИО и должность сотрудника, который выдал КПТ" };

        public void Transpose()
        {
            var rng = Sheet.Range("A1:A4");
            rng.Transpose(XLTransposeOptions.MoveCells);
        }
    }
}
