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
            //У зон не понятно как допол.информацию собирать
            //Подумать над листом кадастровые кварталы

            KPT kpt = new KPT(@"D:\ReaderXml\ReaderXml\КПТ\Примеры\KPT (v10)\50_48_0000000_2016-06-29_kpt10.xml");

            #region Лист сводная информация
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
            #endregion

            #region Лист кадастровые кварталы
            var wsCadastral = wb.Worksheets.Add("Кадастровые кварталы");
            wsCadastral.Cell("A1").Value = "Номер кадастрового квартала";
            wsCadastral.Cell("B1").Value = "Наличие координат";
            wsCadastral.Cell("C1").Value = "Площадь";
            wsCadastral.Cell("A2").Value = kpt.CadastralNumber;
            wsCadastral.Cell("B2").Value = kpt.isCoordinates;
            wsCadastral.Cell("C2").Value = kpt.Area;
            #endregion

            #region Перечисление земельных участков  
            int rowParcels = 6;
            if (kpt.Parcels.Count != 0)
            {
                int r = 2;
                var parcelsExel = from p in kpt.Parcels
                             let cadastralNumberKPT = kpt.CadastralNumber
                             select new {
                                 CadastralNumber = p.CadastralNumber,
                                 CadastralNumberKPT = cadastralNumberKPT,
                                 isCoordinates = p.isCoordinates,
                                 EntSys = p.EntSys,
                                 Name = p.Name,
                                 ParentCadastralNumbers = p.ParentCadastralNumbers,
                                 Area = p.Area,
                                 Category = p.Category,
                                 Utilization = p.Utilization,
                                 Address = p.Address,
                                 CadastralCost = p.CadastralCost
                             };

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

                do
                {
                    foreach (var p in kpt.Parcels)
                    {
                        ws.Cell(rowParcels, "A").Value = p.CadastralNumber;
                        ws.Cell(rowParcels, "A").Hyperlink = new XLHyperlink($"'ЗУ'!A{r++}");
                        ws.Cell(rowParcels, "B").Value = p.Category;
                        ws.Cell(rowParcels++, "C").Value = p.isCoordinates;
                    }
                } while (rowParcels <= kpt.Parcels.Count);

                wsParcels.Cell(2, 1).Value = parcelsExel.AsEnumerable();
            }
            #endregion

            #region Перечисление зданий
            int rowBuilding = rowParcels;
            if (kpt.Buildings.Count != 0)
            {
                int r = 2;

                var buildingsExel = from b in kpt.Buildings
                                  let cadastralNumberKPT = kpt.CadastralNumber
                                  select new
                                  {
                                      CadastralNumber = b.CadastralNumber,
                                      CadastralNumberKPT = cadastralNumberKPT,
                                      isCoordinates = b.isCoordinates,
                                      EntSys = b.EntSys,
                                      Area = b.Area,
                                      ObjectType = b.ObjectType,
                                      Address = b.Address,
                                      CadastralCost = b.CadastralCost
                                  };

                var wsBuildings = wb.Worksheets.Add("Здания");
                wsBuildings.Cell("A1").Value = "Кадастровый номер";
                wsBuildings.Cell("B1").Value = "Номер кадастрового квартала";
                wsBuildings.Cell("C1").Value = "Наличие координат";
                wsBuildings.Cell("D1").Value = "Системы координат";
                wsBuildings.Cell("E1").Value = "Площадь";
                wsBuildings.Cell("F1").Value = "Назначение";
                wsBuildings.Cell("G1").Value = "Адрес";
                wsBuildings.Cell("H1").Value = "Кадастровая стоимость";
                do
                {
                    foreach (var b in kpt.Buildings)
                    {
                        ws.Cell(rowBuilding, "A").Value = b.CadastralNumber;
                        ws.Cell(rowBuilding, "A").Hyperlink = new XLHyperlink($"'Здания'!A{r++}");
                        ws.Cell(rowBuilding, "B").Value = b.ObjectType;
                        ws.Cell(rowBuilding++, "C").Value = b.isCoordinates;
                    }
                } while (rowBuilding < kpt.Buildings.Count + rowParcels);
                wsBuildings.Cell(2, 1).Value = buildingsExel.AsEnumerable();
            }
            #endregion

            #region Перечисление сооружений
            int rowConstruction = rowBuilding;
            if (kpt.Constructions.Count != 0)
            {
                int r = 2;
                var constructionsExel = from c in kpt.Constructions
                                    let cadastralNumberKPT = kpt.CadastralNumber
                                    select new
                                    {
                                        CadastralNumber = c.CadastralNumber,
                                        CadastralNumberKPT = cadastralNumberKPT,
                                        isCoordinates = c.isCoordinates,
                                        EntSys = c.EntSys,
                                        KeyParameters = c.KeyParameters,
                                        ObjectType = c.ObjectType,
                                        Address = c.Address,
                                        CadastralCost = c.CadastralCost
                                    };

                var wsConstructions = wb.Worksheets.Add("Сооружения");
                wsConstructions.Cell("A1").Value = "Кадастровый номер";
                wsConstructions.Cell("B1").Value = "Номер кадастрового квартала";
                wsConstructions.Cell("C1").Value = "Наличие координат";
                wsConstructions.Cell("D1").Value = "Системы координат";
                wsConstructions.Cell("E1").Value = "Характеристики";
                wsConstructions.Cell("F1").Value = "Назначение";
                wsConstructions.Cell("G1").Value = "Адрес";
                wsConstructions.Cell("H1").Value = "Кадастровая стоимость";
                do
                {
                    foreach (var c in kpt.Constructions)
                    {
                        ws.Cell(rowConstruction, "A").Value = c.CadastralNumber;
                        ws.Cell(rowConstruction, "A").Hyperlink = new XLHyperlink($"'Сооружения'!A{r++}");
                        ws.Cell(rowConstruction, "B").Value = c.ObjectType;
                        ws.Cell(rowConstruction++, "C").Value = c.isCoordinates;
                    }
                } while (rowConstruction < kpt.Constructions.Count + rowBuilding);
                wsConstructions.Cell(2, 1).Value = constructionsExel.AsEnumerable();
            }
            #endregion

            #region Перечисление ОНС
            int rowUncompleted = rowConstruction;
            if (kpt.Uncompleteds.Count != 0)
            {
                int r = 2;
                var uncompletedsExel = from u in kpt.Uncompleteds
                                        let cadastralNumberKPT = kpt.CadastralNumber
                                        select new
                                        {
                                            CadastralNumber = u.CadastralNumber,
                                            CadastralNumberKPT = cadastralNumberKPT,
                                            isCoordinates = u.isCoordinates,
                                            EntSys = u.EntSys,
                                            KeyParameters = u.KeyParameters,
                                            ObjectType = u.ObjectType,
                                            Address = u.Address,
                                            CadastralCost = u.CadastralCost
                                        };

                var wsUncompleteds = wb.Worksheets.Add("ОНС");
                wsUncompleteds.Cell("A1").Value = "Кадастровый номер";
                wsUncompleteds.Cell("B1").Value = "Номер кадастрового квартала";
                wsUncompleteds.Cell("C1").Value = "Наличие координат";
                wsUncompleteds.Cell("D1").Value = "Системы координат";
                wsUncompleteds.Cell("E1").Value = "Характеристики";
                wsUncompleteds.Cell("F1").Value = "Назначение";
                wsUncompleteds.Cell("G1").Value = "Адрес";
                wsUncompleteds.Cell("H1").Value = "Кадастровая стоимость";
                do
                {
                    foreach (var u in kpt.Uncompleteds)
                    {
                        ws.Cell(rowUncompleted, "A").Value = u.CadastralNumber;
                        ws.Cell(rowUncompleted, "A").Hyperlink = new XLHyperlink($"'ОНС'!A{r++}");
                        ws.Cell(rowUncompleted, "B").Value = u.ObjectType;
                        ws.Cell(rowUncompleted++, "C").Value = u.isCoordinates;
                    }
                } while (rowUncompleted < kpt.Uncompleteds.Count + rowConstruction);
                wsUncompleteds.Cell(2, 1).Value = uncompletedsExel.AsEnumerable();
            }
            #endregion

            #region Перечисление границ
            int rowBound = rowUncompleted;
            if (kpt.Bounds.Count != 0)
            {
                int r = 2;
                var boundsExel = from b in kpt.Bounds
                                       let cadastralNumberKPT = kpt.CadastralNumber
                                       select new
                                       {
                                           AccountNumber = b.AccountNumber,
                                           CadastralNumberKPT = cadastralNumberKPT,
                                           TypeBoundary = b.TypeBoundary,
                                           Description = b.Description,
                                           isCoordinates = b.isCoordinates,
                                           EntSys = b.EntSys,
                                           AdditionalInformation = b.AdditionalInformation
                                       };

                var wsBounds = wb.Worksheets.Add("Границы");
                wsBounds.Cell("A1").Value = "Учетный номер";
                wsBounds.Cell("B1").Value = "Номер кадастрового квартала";
                wsBounds.Cell("C1").Value = "Вид границы";
                wsBounds.Cell("D1").Value = "Наименование";
                wsBounds.Cell("E1").Value = "Наличие координат";
                wsBounds.Cell("F1").Value = "Системы координат";
                wsBounds.Cell("G1").Value = "Дополнительная информация";
                do
                {
                    foreach (var b in kpt.Bounds)
                    {
                        ws.Cell(rowBound, "A").Value = b.AccountNumber;
                        ws.Cell(rowBound, "A").Hyperlink = new XLHyperlink($"'Границы'!A{r++}");
                        ws.Cell(rowBound, "B").Value = b.TypeBoundary;
                        ws.Cell(rowBound++, "C").Value = b.isCoordinates;
                    }
                } while (rowBound < kpt.Bounds.Count + rowUncompleted);
                wsBounds.Cell(2, 1).Value = boundsExel.AsEnumerable();
            }
            #endregion

            #region Перечисление зон
            int rowZone = rowBound;
            if (kpt.Zones.Count != 0)
            {
                int r = 2;
                var zonesExel = from z in kpt.Zones
                                 let cadastralNumberKPT = kpt.CadastralNumber
                                 select new
                                 {
                                     AccountNumber = z.AccountNumber,
                                     CadastralNumberKPT = cadastralNumberKPT,
                                     TypeZone = z.TypeZone,
                                     Description = z.Description,
                                     isCoordinates = z.isCoordinates,
                                     EntSys = z.EntSys,
                                     AdditionalInformation = z.AdditionalInformation
                                 };

                var wsZones = wb.Worksheets.Add("Зоны");
                wsZones.Cell("A1").Value = "Учетный номер";
                wsZones.Cell("B1").Value = "Номер кадастрового квартала";
                wsZones.Cell("C1").Value = "Вид зоны";
                wsZones.Cell("D1").Value = "Наименование";
                wsZones.Cell("E1").Value = "Наличие координат";
                wsZones.Cell("F1").Value = "Системы координат";
                wsZones.Cell("G1").Value = "Дополнительная информация";
                do
                {
                    foreach (var z in kpt.Zones)
                    {
                        ws.Cell(rowZone, "A").Value = z.AccountNumber;
                        ws.Cell(rowZone, "A").Hyperlink = new XLHyperlink($"'Зоны'!A{r++}");
                        ws.Cell(rowZone, "B").Value = z.TypeZone;
                        ws.Cell(rowZone++, "C").Value = z.isCoordinates;
                    }
                } while (rowZone < kpt.Zones.Count + rowBound);
                wsZones.Cell(2, 1).Value = zonesExel.AsEnumerable();
            }
            #endregion

            #region Перечисление пунктов ОМС
            int rowOMSPoint = rowZone;
            if (kpt.OMSPoints.Count != 0)
            {
                int r = 2;
                var omsExel = from o in kpt.OMSPoints
                                let cadastralNumberKPT = kpt.CadastralNumber
                                select new
                                {
                                    PNmb = o.PNmb,
                                    PName = o.PName,
                                    PKlass = o.PKlass,
                                    OrdX = o.OrdX,
                                    OrdY = o.OrdY,
                                    CadastralNumberKPT = cadastralNumberKPT 
                                };

                var wsOMSPoint = wb.Worksheets.Add("Пункты ОМС");
                wsOMSPoint.Cell("A1").Value = "Номер";
                wsOMSPoint.Cell("B1").Value = "Название и тип";
                wsOMSPoint.Cell("C1").Value = "Класс";
                wsOMSPoint.Cell("D1").Value = "X";
                wsOMSPoint.Cell("E1").Value = "Y";
                wsOMSPoint.Cell("F1").Value = "Номер кадастрового квартала";
                do
                {
                    foreach (var o in kpt.OMSPoints)
                    {
                        ws.Cell(rowOMSPoint, "A").Value = o.PNmb;
                        ws.Cell(rowOMSPoint++, "A").Hyperlink = new XLHyperlink($"'Пункты ОМС'!A{r++}");
                        //ws.Cell(rowOMSPoint, "B").Value = o.TypeZone;
                        //ws.Cell(rowOMSPoint++, "C").Value = o.isCoordinates;
                    }
                } while (rowOMSPoint < kpt.OMSPoints.Count + rowZone);
                wsOMSPoint.Cell(2, 1).Value = omsExel.AsEnumerable();
            }
            #endregion

            wb.SaveAs("1.xlsx");
            System.Diagnostics.Process.Start("1.xlsx");
        }

    }
}
