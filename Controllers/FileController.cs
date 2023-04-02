using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private static IWebHostEnvironment _environment { get; set; }
        public FileController(IWebHostEnvironment webHostEnvironment) { _environment = webHostEnvironment; }

        [HttpPost("{table}/{name}")]
        public ActionResult<string> Post([FromForm] IFormFile file, string table, string name)
        {
            //int fileId = new MyDBContext(table).GetMaxId();
            try
            {
                if (file == null)
                    return BadRequest("Need File.");
                if (file.Length <= 0)
                    return BadRequest("File is to small.");
                string path = Path.Combine(_environment.WebRootPath, "uploads", table);
                string fileName = name + Path.GetExtension(file.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = System.IO.File.Create(Path.Combine(path, fileName)))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    return Ok(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{table}/{id}")]
        public ActionResult<string> Get(string table, string id)
        {
            string path = Path.Combine(_environment.WebRootPath, "uploads", table);
            if(!Directory.Exists(path))
                return BadRequest("Image Not Found.");
            string filePath = Directory.GetFiles(path).ToList().Find(f => Path.GetFileName(f) == id.ToString()); // Path.GetFileNameWithoutExtension(f)
            if (string.IsNullOrEmpty(filePath)) return BadRequest("Image Not Found.");

            byte[] b = System.IO.File.ReadAllBytes(filePath);
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var fileType);
            return File(b, fileType);
        }
    }
}
