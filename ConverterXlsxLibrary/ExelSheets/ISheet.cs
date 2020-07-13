using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Интерфейс заполнения рабочего листа Exel.
    /// </summary>
    public interface ISheet
    {
        /// <summary>
        /// Рабочий лист Exel.
        /// </summary>
        IXLWorksheet Sheet { get; set; }

        /// <summary>
        /// Заголовок рабочего листа.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Перечисление колонок, содержащихся в рабочем листе.
        /// </summary>
        List<string> Column { get; }

        /// <summary>
        /// Заполнение рабочего листа сведениями о кадастровых объектах.
        /// </summary>
        /// <param name="KPT">Необходимые данные для заполнения.</param>
        void Fill(CadastralPlanTerritory KPT);

        /// <summary>
        /// Добавление информации о кадастровых объектах на сводный лист.
        /// </summary>
        /// <param name="KPT">Необходимые данные для заполнения.</param>
        /// <param name="XlWorkbook">Сводный лист.</param>
        /// <param name="numObjKpt">Строка, с которой начинается заполнение сводного листа.</param>
        void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt);
    }
}
