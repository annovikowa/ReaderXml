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
            string id = "";
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                //записать все в бд, получить id
                id = System.Guid.NewGuid().ToString();
                Conversion conversion = new Conversion(id, path, Status.Created.ToString());
                database.Context.Conversions.Add(conversion);                
                database.SaveChanges();
            }

            //запустить конвертацию: реализовать для этого очередь (в отдельном потоке), примеры можно легко найти
            ConverterHelper converterHelper = new ConverterHelper();
            Task<ExelFiller>.Run(() =>
               {
                  converterHelper.Convertions(path, id);
               });
            return Ok(id);
        }
        [HttpGet]
        public IHttpActionResult GetStatus(string id)
        {
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                //добавить в репозиторий метод поиска по id, найден - вернуть Ok(status), не найден - NotFound(). Если есть ошибки - вернуть и их.
                var errors = database.GetError(id);                
                if (errors != null)
                {

                }
                var result = database.GetConversion(id);
                if (result != null)
                    return Ok(result.Status);
                else
                    return NotFound();
            }
            
        }
        [HttpGet]
        public HttpResponseMessage GetFile(string id)
        {
            //получить путь из БД
            string path = "";
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                path = database.GetConversion(id).PathOutput;
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