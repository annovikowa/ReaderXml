using ClosedXML.Excel;
using ConverterXlsxLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace ConverterXlsxLibrary.ExelSheets
{
    /// <summary>
    /// Лист "Проект межевания".
    /// </summary>
    public class SurveyingProjectSheet : ISheet
    {
        public IXLWorksheet Sheet { get; set; }

        public string Title => "Проект межевания";

        public List<string> Column => new List<string>() { "Номер кадастрового квартала", "Условный номер ЗУ", "Наличие координат", "Системы координат" };

        /// <summary>
        /// Строка, с которой начинается заполнение рабочего листа.
        /// </summary>
        private int _numberSurveyingProject { get; set; } = 2;

        public void AddHiberLinks(CadastralPlanTerritory KPT, XLWorkbook XlWorkbook, ref int numObjKpt)
        {
            int numSurveyingProject = 1;
            var sheet = XlWorkbook.Worksheet("Сводная информация");
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                do
                {
                    foreach (var s in cadastralBlock.SurveyingProjects)
                    {
                        sheet.Cell(numObjKpt, "A").Value = s.SurveyProjectNum;
                        sheet.Cell(numObjKpt, "A").Hyperlink = new XLHyperlink($"'{Title}'!A{_numberSurveyingProject++}");
                        sheet.Cell(numObjKpt++, "B").Value = "Проект межевания";
                        numSurveyingProject++;
                    }
                } while (numSurveyingProject <= cadastralBlock.SurveyingProjects.Count);
            }
        }

        public void Fill(CadastralPlanTerritory KPT)
        {
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                var surveyingProjectExel = from s in cadastralBlock.SurveyingProjects
                                           select new
                                           {
                                               SurveyProjectNum = s.SurveyProjectNum,
                                               CadastralNumber = s.CadasstralBlockNumber,
                                               NominalNumber = s.NominalNumber,
                                               HasCoordinates = s.HasCoordinates,
                                               CoorSys = s.CoorSys
                                           };
                Sheet.Cell(_numberSurveyingProject, 1).Value = surveyingProjectExel.AsEnumerable();
            }
        }
    }
}
