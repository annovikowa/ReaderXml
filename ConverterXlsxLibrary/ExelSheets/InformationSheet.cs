using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Лист "Сводная информация".
    /// </summary>
    public class InformationSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title { get; } = "Сводная информация";

        public List<string> Column { get; } = new List<string>() { "Имя XML-файла", "Наименование органа регистрации прав", "Номер и дата выдачи КПТ", "ФИО и должность сотрудника, который выдал КПТ" };

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            Sheet.Cell("A4").Value = "Кадастровый номер";
            Sheet.Cell("B4").Value = "Вид объекта";
            Sheet.Cell("C4").Value = "Наличие или остутствие координат";

            Sheet.Cell("A2").Value = KPT.FileName;
            Sheet.Cell("B2").Value = KPT.OrganRegistrRights;
            Sheet.Cell("C2").Value = KPT.DateFormation;
            Sheet.Cell("C2").Style.DateFormat.Format = "yyyy-MM-dd";
            Sheet.Cell("C2").Value += ", " + KPT.RegistrationNumber;
            Sheet.Cell("D2").Value = KPT.Official; 
        }
    }
}
