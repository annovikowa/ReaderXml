using ClosedXML.Excel;
using ReaderXml;
using ReaderXml.ExelSheets;
using System.Web.Http;

namespace ConverterXlsx
{
    public class XlsxController : ApiController
    {
        public XLWorkbook Get(string path)
        {
            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read(path);

            ExelFiller exelFiller = new ExelFiller(KPT);
            exelFiller.XlWorkbook.SaveAs("test.xlsx");
            return exelFiller.XlWorkbook;
        }
    }
}