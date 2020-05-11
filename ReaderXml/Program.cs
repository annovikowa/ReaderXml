using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using ClosedXML.Excel;
using ReaderXml.KPT;
using ReaderXml.ECPT;


namespace ReaderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtractCadastralPlanTerritory ECPT = new ExtractCadastralPlanTerritory(@"D:\ReaderXml\ReaderXml\КПТ\Примеры\extract_cadastral_plan_territory\24_54_0105002_2017-06-01_kpt11.xml");

            CadastralPlanTerritory KPT = new CadastralPlanTerritory(@"D:\ReaderXml\ReaderXml\КПТ\Примеры\KPT (v10)\50_48_0000000_2016-06-29_kpt10.xml");


            #region Лист сводная информация
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Сводная информация");
            ws.Cell("A1").Value = "Имя xml-файла";
            ws.Cell("B1").Value = "Наименование органа регистрации прав";
            ws.Cell("C1").Value = "Номер и дата выдачи КПТ";
            ws.Cell("D1").Value = "ФИО и должность сотрудника, который выдал КПТ"; //Может как в схеме названть "Должностное лицо"?
            ws.Cell("B2").Value = KPT.Organization;
            ws.Cell("C2").Value = KPT.Date;
            ws.Cell("C2").Style.DateFormat.Format = "yyyy-MM-dd";
            ws.Cell("C2").Value += ", " + KPT.Number;
            ws.Cell("D2").Value = KPT.Official;
            #endregion

            #region Лист кадастровые кварталы
            var wsCadastral = wb.Worksheets.Add("Кадастровые кварталы");
            wsCadastral.Cell("A1").Value = "Номер кадастрового квартала";
            wsCadastral.Cell("B1").Value = "Наличие координат";
            wsCadastral.Cell("C1").Value = "Площадь";
            #endregion

            IXLWorksheet wsParcels = null, wsBuildings = null, wsConstructions = null, wsUncompleteds = null, wsBounds = null, wsZones = null, wsOMSPoint = null;
            int numberObjKpt = 3;
            int quarter = 2;
            int numberParcels = 2, numberBuilding = 2, numberConstruction = 2, numberUncompleted = 2, numberBound = 2, numberZone = 2, numberOMS = 2;

            foreach (var kpt in KPT.CadastralBlocks)
            {
                #region Перечисление земельных участков
                if (kpt.Parcels.Count > 0)
                {
                    try
                    {
                        wsParcels = wb.Worksheets.Add("ЗУ");
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
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление зданий
                if (kpt.Buildings.Count > 0)
                {
                    try
                    {
                        wsBuildings = wb.Worksheets.Add("Здания");
                        wsBuildings.Cell("A1").Value = "Кадастровый номер";
                        wsBuildings.Cell("B1").Value = "Номер кадастрового квартала";
                        wsBuildings.Cell("C1").Value = "Наличие координат";
                        wsBuildings.Cell("D1").Value = "Системы координат";
                        wsBuildings.Cell("E1").Value = "Площадь";
                        wsBuildings.Cell("F1").Value = "Назначение";
                        wsBuildings.Cell("G1").Value = "Адрес";
                        wsBuildings.Cell("H1").Value = "Кадастровая стоимость";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление сооружений
                if (kpt.Constructions.Count > 0)
                {
                    try
                    {
                        wsConstructions = wb.Worksheets.Add("Сооружения");
                        wsConstructions.Cell("A1").Value = "Кадастровый номер";
                        wsConstructions.Cell("B1").Value = "Номер кадастрового квартала";
                        wsConstructions.Cell("C1").Value = "Наличие координат";
                        wsConstructions.Cell("D1").Value = "Системы координат";
                        wsConstructions.Cell("E1").Value = "Характеристики";
                        wsConstructions.Cell("F1").Value = "Назначение";
                        wsConstructions.Cell("G1").Value = "Адрес";
                        wsConstructions.Cell("H1").Value = "Кадастровая стоимость";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление ОНС
                if (kpt.Uncompleteds.Count > 0)
                {
                    try
                    {
                        wsUncompleteds = wb.Worksheets.Add("ОНС");
                        wsUncompleteds.Cell("A1").Value = "Кадастровый номер";
                        wsUncompleteds.Cell("B1").Value = "Номер кадастрового квартала";
                        wsUncompleteds.Cell("C1").Value = "Наличие координат";
                        wsUncompleteds.Cell("D1").Value = "Системы координат";
                        wsUncompleteds.Cell("E1").Value = "Характеристики";
                        wsUncompleteds.Cell("F1").Value = "Назначение";
                        wsUncompleteds.Cell("G1").Value = "Адрес";
                        wsUncompleteds.Cell("H1").Value = "Кадастровая стоимость";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление границ
                if (kpt.Bounds.Count > 0)
                {
                    try
                    {
                        wsBounds = wb.Worksheets.Add("Границы");
                        wsBounds.Cell("A1").Value = "Учетный номер";
                        wsBounds.Cell("B1").Value = "Номер кадастрового квартала";
                        wsBounds.Cell("C1").Value = "Вид границы";
                        wsBounds.Cell("D1").Value = "Наименование";
                        wsBounds.Cell("E1").Value = "Наличие координат";
                        wsBounds.Cell("F1").Value = "Системы координат";
                        wsBounds.Cell("G1").Value = "Дополнительная информация";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление зон
                if (kpt.Zones.Count > 0)
                {
                    try
                    {
                        wsZones = wb.Worksheets.Add("Зоны");
                        wsZones.Cell("A1").Value = "Учетный номер";
                        wsZones.Cell("B1").Value = "Номер кадастрового квартала";
                        wsZones.Cell("C1").Value = "Вид зоны";
                        wsZones.Cell("D1").Value = "Наименование";
                        wsZones.Cell("E1").Value = "Наличие координат";
                        wsZones.Cell("F1").Value = "Системы координат";
                        wsZones.Cell("G1").Value = "Дополнительная информация";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region Перечисление пунктов ОМС   
                if (kpt.OMSPoints.Count > 0)
                {
                    try
                    {
                        wsOMSPoint = wb.Worksheets.Add("Пункты ОМС");
                        wsOMSPoint.Cell("A1").Value = "Номер";
                        wsOMSPoint.Cell("B1").Value = "Название и тип";
                        wsOMSPoint.Cell("C1").Value = "Класс";
                        wsOMSPoint.Cell("D1").Value = "X";
                        wsOMSPoint.Cell("E1").Value = "Y";
                        wsOMSPoint.Cell("F1").Value = "Номер кадастрового квартала";
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion
            }
            foreach (var kpt in KPT.CadastralBlocks)
            {
                #region Лист кадастровые кварталы
                wsCadastral.Cell(quarter, "A").Value = kpt.CadastralNumber;
                wsCadastral.Cell(quarter, "B").Value = kpt.isCoordinates;
                wsCadastral.Cell(quarter++, "C").Value = kpt.Area;
                #endregion

                #region Перечисление земельных участков  

                if (wsParcels != null)
                {
                    int numParcel = 1;
                    var parcelsExel = from p in kpt.Parcels
                                      let cadastralNumberKPT = kpt.CadastralNumber
                                      select new
                                      {
                                          CadastralNumber = p.CadastralNumber,
                                          CadastralNumberKPT = cadastralNumberKPT,
                                          isCoordinates = p.isCoordinates,
                                          EntSys = p.CoorSys,
                                          Name = p.Name,
                                          ParentCadastralNumbers = p.ParentCadastralNumbers,
                                          Area = p.Area,
                                          Category = p.Category,
                                          Utilization = p.Utilization,
                                          Address = p.Address,
                                          CadastralCost = p.CadastralCost
                                      };

                    wsParcels.Cell(numberParcels, 1).Value = parcelsExel.AsEnumerable();
                    do
                    {
                        foreach (var p in kpt.Parcels)
                        {
                            ws.Cell(numberObjKpt, "A").Value = p.CadastralNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'ЗУ'!A{numberParcels++}");
                            ws.Cell(numberObjKpt, "B").Value = p.Category;
                            ws.Cell(numberObjKpt++, "C").Value = p.isCoordinates;
                            numParcel++;
                        }
                    } while (numParcel <= kpt.Parcels.Count);
                }
                #endregion

                #region Перечисление зданий
                if (kpt.Buildings.Count != 0)
                {
                    int numBuilding = 1;
                    var buildingsExel = from b in kpt.Buildings
                                        let cadastralNumberKPT = kpt.CadastralNumber
                                        select new
                                        {
                                            CadastralNumber = b.CadastralNumber,
                                            CadastralNumberKPT = cadastralNumberKPT,
                                            isCoordinates = b.isCoordinates,
                                            EntSys = b.CoorSys,
                                            Area = b.Area,
                                            ObjectType = b.ObjectType,
                                            Address = b.Address,
                                            CadastralCost = b.CadastralCost
                                        };

                    wsBuildings.Cell(numberBuilding, 1).Value = buildingsExel.AsEnumerable();
                    do
                    {
                        foreach (var b in kpt.Buildings)
                        {
                            ws.Cell(numberObjKpt, "A").Value = b.CadastralNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'Здания'!A{numberBuilding++}");
                            ws.Cell(numberObjKpt, "B").Value = b.ObjectType;
                            ws.Cell(numberObjKpt++, "C").Value = b.isCoordinates;
                            numBuilding++;
                        }
                    } while (numBuilding <= kpt.Buildings.Count);
                }
                #endregion

                #region Перечисление сооружений                
                if (kpt.Constructions.Count != 0)
                {
                    int numConstruction = 1;
                    var constructionsExel = from c in kpt.Constructions
                                            let cadastralNumberKPT = kpt.CadastralNumber
                                            select new
                                            {
                                                CadastralNumber = c.CadastralNumber,
                                                CadastralNumberKPT = cadastralNumberKPT,
                                                isCoordinates = c.isCoordinates,
                                                EntSys = c.CoorSys,
                                                KeyParameters = c.KeyParameters,
                                                ObjectType = c.ObjectType,
                                                Address = c.Address,
                                                CadastralCost = c.CadastralCost
                                            };
                    wsConstructions.Cell(numberConstruction, 1).Value = constructionsExel.AsEnumerable();
                    do
                    {
                        foreach (var c in kpt.Constructions)
                        {
                            ws.Cell(numberObjKpt, "A").Value = c.CadastralNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'Сооружения'!A{numberConstruction++}");
                            ws.Cell(numberObjKpt, "B").Value = c.ObjectType;
                            ws.Cell(numberObjKpt++, "C").Value = c.isCoordinates;
                            numConstruction++;
                        }
                    } while (numConstruction <= kpt.Constructions.Count);
                }
                #endregion

                #region Перечисление ОНС
                if (kpt.Uncompleteds.Count != 0)
                {
                    int numUncompleted = 1;
                    var uncompletedsExel = from u in kpt.Uncompleteds
                                           let cadastralNumberKPT = kpt.CadastralNumber
                                           select new
                                           {
                                               CadastralNumber = u.CadastralNumber,
                                               CadastralNumberKPT = cadastralNumberKPT,
                                               isCoordinates = u.isCoordinates,
                                               EntSys = u.CoorSys,
                                               KeyParameters = u.KeyParameters,
                                               ObjectType = u.ObjectType,
                                               Address = u.Address,
                                               CadastralCost = u.CadastralCost
                                           };
                    wsUncompleteds.Cell(numberUncompleted, 1).Value = uncompletedsExel.AsEnumerable();
                    do
                    {
                        foreach (var u in kpt.Uncompleteds)
                        {
                            ws.Cell(numberObjKpt, "A").Value = u.CadastralNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'ОНС'!A{numberUncompleted++}");
                            ws.Cell(numberObjKpt, "B").Value = u.ObjectType;
                            ws.Cell(numberObjKpt++, "C").Value = u.isCoordinates;
                            numUncompleted++;
                        }
                    } while (numUncompleted <= kpt.Uncompleteds.Count);
                }
                #endregion

                #region Перечисление границ
                if (kpt.Bounds.Count != 0)
                {
                    int numBound = 1;
                    var boundsExel = from b in kpt.Bounds
                                     let cadastralNumberKPT = kpt.CadastralNumber
                                     select new
                                     {
                                         AccountNumber = b.AccountNumber,
                                         CadastralNumberKPT = cadastralNumberKPT,
                                         TypeBoundary = b.TypeBoundary,
                                         Description = b.Description,
                                         isCoordinates = b.isCoordinates,
                                         EntSys = b.CoorSys,
                                         AdditionalInformation = b.AdditionalInformation
                                     };
                    wsBounds.Cell(numberBound, 1).Value = boundsExel.AsEnumerable();
                    do
                    {
                        foreach (var b in kpt.Bounds)
                        {
                            ws.Cell(numberObjKpt, "A").Value = b.AccountNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'Границы'!A{numberBound++}");
                            ws.Cell(numberObjKpt, "B").Value = b.TypeBoundary;
                            ws.Cell(numberObjKpt++, "C").Value = b.isCoordinates;
                            numBound++;
                        }
                    } while (numBound <= kpt.Bounds.Count);
                }
                #endregion

                #region Перечисление зон
                if (kpt.Zones.Count != 0)
                {
                    int numZone = 1;
                    var zonesExel = from z in kpt.Zones
                                    let cadastralNumberKPT = kpt.CadastralNumber
                                    select new
                                    {
                                        AccountNumber = z.AccountNumber,
                                        CadastralNumberKPT = cadastralNumberKPT,
                                        TypeZone = z.TypeZone,
                                        Description = z.Description,
                                        isCoordinates = z.isCoordinates,
                                        EntSys = z.CoorSys,
                                        AdditionalInformation = z.AdditionalInformation
                                    };
                    wsZones.Cell(numberZone, 1).Value = zonesExel.AsEnumerable();
                    do
                    {
                        foreach (var z in kpt.Zones)
                        {
                            ws.Cell(numberObjKpt, "A").Value = z.AccountNumber;
                            ws.Cell(numberObjKpt, "A").Hyperlink = new XLHyperlink($"'Зоны'!A{numberZone++}");
                            ws.Cell(numberObjKpt, "B").Value = z.TypeZone;
                            ws.Cell(numberObjKpt++, "C").Value = z.isCoordinates;
                            numZone++;
                        }
                    } while (numZone <= kpt.Zones.Count);
                }
                #endregion

                #region Перечисление пунктов ОМС
                if (kpt.OMSPoints.Count != 0)
                {
                    int numOMS = 2;
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
                    wsOMSPoint.Cell(numberOMS, 1).Value = omsExel.AsEnumerable();
                    do
                    {
                        foreach (var o in kpt.OMSPoints)
                        {
                            ws.Cell(numberObjKpt, "A").Value = o.PNmb;
                            ws.Cell(numberObjKpt++, "A").Hyperlink = new XLHyperlink($"'Пункты ОМС'!A{numberOMS++}");
                            //ws.Cell(numberObjKpt, "B").Value = o.TypeZone;
                            //ws.Cell(numberObjKpt++, "C").Value = o.isCoordinates;
                            numOMS++;
                        }
                    } while (numOMS <= kpt.OMSPoints.Count);
                }
                #endregion
            }
            wb.SaveAs("1.xlsx");
            System.Diagnostics.Process.Start("1.xlsx");
        }

    }
}
