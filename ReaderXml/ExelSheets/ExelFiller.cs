using ClosedXML.Excel;
using ReaderXml.Models;
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


        
        public ExelFiller(CadastralPlanTerritory KPT)
        {
            int numObjKpt = 5;
            FillSheet(new InformationSheet(), KPT, ref numObjKpt);
            FillSheet(new CadastralSheet(), KPT, ref numObjKpt);
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                if (cadastralBlock.Parcels.Count != 0)
                {
                    FillSheet(new ParcelSheet(), KPT, ref numObjKpt);
                }

                if(cadastralBlock.Buildings.Count != 0)
                {
                    FillSheet(new BuildingSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.Constructions.Count != 0)
                {
                    FillSheet(new ConstructionSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.Uncompleteds.Count != 0)
                {
                    FillSheet(new UncompletedSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.Bounds.Count != 0)
                {
                    FillSheet(new BoundSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.Zones.Count != 0)
                {
                    FillSheet(new ZoneSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.SurveyingProjects.Count != 0)
                {
                    FillSheet(new SurveyingProjectSheet(), KPT, ref numObjKpt);
                }

                if (cadastralBlock.OMSPoints.Count != 0)
                {
                    FillSheet(new OMSPointSheet(), KPT, ref numObjKpt);
                }
            }
        }

        public void FillSheet<T>(T obj, CadastralPlanTerritory KPT, ref int numObjKpt) where T : ISheet, new()
        {
            obj = new T();
            obj.Sheet = XlWorkbook.Worksheets.Add(obj.Title);
            obj.Sheet.Cell(1, 1).Value = obj.Column.AsEnumerable();
            var rng = obj.Sheet.Range("A1:A20");
            rng.Transpose(XLTransposeOptions.MoveCells);
            obj.Fill(KPT);
            obj.AddHiberLinks(KPT, XlWorkbook, ref numObjKpt);
        }
    }
}
