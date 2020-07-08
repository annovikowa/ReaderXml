using ClosedXML.Excel;
using ConverterXlsx.DB;
using ReaderXml;
using ReaderXml.ExelSheets;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ConverterXlsx.Controllers
{
    public class XlsxController : ApiController
    {
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count != 1)
            {
                return BadRequest("Передано некорректное количество файлов");
            }
            var file = files[0];
            var path = System.Web.Hosting.HostingEnvironment.MapPath("some_path"); //тут надо указать папку для файлов
            //дополнительно добавить имя
            file.SaveAs(path);
            long id = 1;
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                
                //записать все в бд, получить id
                database.SaveChanges();
            }
            //запустить конвертацию: реализовать для этого очередь (в отдельном потоке), примеры можно легко найти
            return Ok(id);
        }
        [HttpGet]
        public IHttpActionResult GetStatus(long id)
        {
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                //добавить в репозиторий метод поиска по id, найден - вернуть Ok(status), не найден - NotFound(). Если есть ошибки - вернуть и их.
                return Ok();
            }
            
        }
        [HttpGet]
        public HttpResponseMessage GetFile(long id)
        {
            //получить путь из БД
            var path = @"C:\Temp\test.exe";
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