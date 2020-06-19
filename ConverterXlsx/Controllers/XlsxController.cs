using ClosedXML.Excel;
using ReaderXml;
using ReaderXml.ExelSheets;
using System.Web.Http;
using System.Web.Mvc;

namespace ConverterXlsx.Controllers
{
    public class XlsxController : ApiController
    {
        public ActionResult Get(string path)
        {
            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read(path);

            ExelFiller exelFiller = new ExelFiller(KPT);
            exelFiller.XlWorkbook.SaveAs("test.xlsx");
            return ;
        }
    }
}