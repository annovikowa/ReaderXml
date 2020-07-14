using ClosedXML.Excel;
using ConverterXlsx.DB;
using ConverterXlsxLibrary.ExelSheets;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Threading;
using ConverterXlsx.DB.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ConverterXlsx.Controllers
{
    public class XlsxController : ApiController
    {     

        [HttpPost]
        [Route("upload")]
        public IHttpActionResult UploadFile()
        {
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count != 1)
            {
                return BadRequest("Передано некорректное количество файлов");
            }
            var file = files[0];
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/some_path"); //тут надо указать папку для файлов
            //дополнительно добавить имя
            file.SaveAs(path);
            var id = "";
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                //записать все в бд, получить id
                Conversion conversion = new Conversion(path, Status.Created);
                database.Context.Conversions.Add(conversion);
                database.SaveChanges();
                id = conversion.ConversionId;
            }

            //запустить конвертацию: реализовать для этого очередь (в отдельном потоке), примеры можно легко найти
            ConverterHelper converterHelper = new ConverterHelper();
            StartConvertionAsync(path, id, converterHelper);
            return Ok(id);
        }

        private static async Task StartConvertionAsync(string path, string id, ConverterHelper converterHelper)
        {
            await Task.Run(() =>
            {
                converterHelper.Convertions(path, id);
            });
        }

        [HttpGet]
        [Route("status")]
        public IHttpActionResult GetStatus(string id)
        {
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                //добавить в репозиторий метод поиска по id, найден - вернуть Ok(status), не найден - NotFound(). Если есть ошибки - вернуть и их.
                var result = database.GetConversion(id);
                if (result != null)
                    return Ok(new { result.Status, result.Errors });
                else
                    return NotFound();
            }
            
        }
        [HttpGet]
        [Route("result")]
        public HttpResponseMessage GetFile(string id)
        {
            //получить путь из БД
            string path = "";
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                path = database.GetConversion(id)?.PathOutput;
            }
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }    
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(path)
            };

            return result;
        }
    }
}