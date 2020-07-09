using ReaderXml;
using ReaderXml.ExelSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConverterXlsx
{
    public class ConverterHelper
    {
        public void Convertions(object path)
        {
            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read((string)path);

            ExelFiller exelFiller = new ExelFiller(KPT);
            //exelFiller.XlWorkbook.SaveAs("test.xlsx");
        }
    }
}