using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using ClosedXML.Excel;

namespace ReaderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            KPT kpt = new KPT(@"D:\ReaderXml\ReaderXml\КПТ\Примеры\KPT (v10)\50_48_0000000_2016-06-29_kpt10.xml");

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Сводная информация");
            ws.Cell("A1").Value = "Имя xml-файла";
            ws.Cell("B1").Value = "Наименование органа регистрации прав";
            ws.Cell("C1").Value = "Номер и дата выдачи КПТ";
            ws.Cell("D1").Value = "ФИО и должность сотрудника, который выдал КПТ"; //Может как в схеме названть "Должностное лицо"?

            ws.Cell("A5").Value = "Кадастровый номер";
            ws.Cell("B5").Value = "Вид объекта";
            ws.Cell("C5").Value = "Наличие/отсутствие координат";

            ws.Cell("B2").Value = kpt.Organization;
            ws.Cell("C2").Value = kpt.Date;
            ws.Cell("C2").Style.DateFormat.Format = "yyyy-MM-dd";
            ws.Cell("C2").Value += ", " + kpt.Number;
            ws.Cell("D2").Value = kpt.Official;

            int rowParcels = 6;
            if (kpt.Parcels.Count != 0)
            {
                int r = 2;
                var wsParcels = wb.Worksheets.Add("ЗУ");
                wsParcels.Cell("A1").Value = "Кадастровый номер";
                wsParcels.Cell("B1").Value = "Номер кадастрового квартала";
                wsParcels.Cell("C1").Value = "Наличие координат";
                wsParcels.Cell("D1").Value = "Системы координат";
                wsParcels.Cell("E1").Value = "Вид ЗУ";
                wsParcels.Cell("F1").Value = "Кадастровый номер ЕЗП (единого землепользования)";
                wsParcels.Cell("G1").Value = "Площадь";
                wsParcels.Cell("H1").Value = "Категория земель";
                wsParcels.Cell("I1").Value = "Виды разрешенного использования";
                wsParcels.Cell("J1").Value = "Адрес";
                wsParcels.Cell("K1").Value = "Кадастровая стоимость";
                #region Перечисление земельных участков                
                do
                {
                    foreach (var p in kpt.Parcels)
                    {
                        ws.Cell(rowParcels, "A").Value = p.CadastralNumber;
                        ws.Cell(rowParcels, "A").Hyperlink = new XLHyperlink($"'ЗУ'!A{r}");
                        ws.Cell(rowParcels, "B").Value = p.Category;
                        ws.Cell(rowParcels++, "C").Value = p.isCoordinates;

                        wsParcels.Cell(r, "A").Value = p.CadastralNumber;
                        wsParcels.Cell(r, "B").Value = kpt.CadastralNumber;
                        wsParcels.Cell(r, "C").Value = p.isCoordinates;
                        wsParcels.Cell(r, "D").Value = p.EntSys;
                        wsParcels.Cell(r, "E").Value = p.Name;
                        wsParcels.Cell(r, "F").Value = p.ParentCadastralNumbers;
                        wsParcels.Cell(r, "G").Value = p.Area;
                        wsParcels.Cell(r, "H").Value = p.Category;
                        wsParcels.Cell(r, "I").Value = "";
                        wsParcels.Cell(r, "G").Value = "";
                        wsParcels.Cell(r++, "K").Value = p.CadastralCost;
                    }
                } while (rowParcels <= kpt.Parcels.Count);
                #endregion
            }


            #region Перечисление зданий
            int rowBuilding = rowParcels;
            do
            {
                foreach (var b in kpt.Buildings)
                {
                    ws.Cell(rowBuilding, "A").Value = b.CadastralNumber;
                    ws.Cell(rowBuilding, "B").Value = b.ObjectType;
                    ws.Cell(rowBuilding++, "C").Value = b.isCoordinates;
                }
            } while (rowBuilding < kpt.Buildings.Count + rowParcels);
            #endregion

            #region Перечисление сооружений
            int rowConstruction = rowBuilding;
            do
            {
                foreach (var c in kpt.Constructions)
                {
                    ws.Cell(rowConstruction, "A").Value = c.CadastralNumber;
                    ws.Cell(rowConstruction, "B").Value = c.ObjectType;
                    ws.Cell(rowConstruction++, "C").Value = c.isCoordinates;
                }
            } while (rowConstruction < kpt.Constructions.Count + rowBuilding);
            #endregion

            #region Перечисление ОНС
            int rowUncompleted = rowConstruction;
            do
            {
                foreach (var c in kpt.Uncompleteds)
                {
                    ws.Cell(rowUncompleted, "A").Value = c.CadastralNumber;
                    ws.Cell(rowUncompleted, "B").Value = c.ObjectType;
                    ws.Cell(rowUncompleted++, "C").Value = c.isCoordinates;
                }
            } while (rowUncompleted < kpt.Uncompleteds.Count + rowConstruction);
            #endregion
            //ws.Columns(1, 4).AdjustToContents();
            wb.SaveAs("1.xlsx");

            System.Diagnostics.Process.Start("1.xlsx");
        }

    }
}
