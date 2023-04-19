using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace EmergencySituations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private static IWebHostEnvironment _environment { get; set; }
        public FileController(IWebHostEnvironment webHostEnvironment) { _environment = webHostEnvironment; }

        [HttpPost("{table}/{id}")]
        [AuthFilter]
        public ActionResult<string> Post([FromForm] IFormFile file, string table, string id)
        {
            if (file == null)
                return BadRequest("Need File.");
            if (file.Length <= 0)
                return BadRequest("File is to small.");
            string path = Path.Combine(_environment.WebRootPath, "uploads", table);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = id + Path.GetExtension(file.FileName);
            using (FileStream fileStream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return Ok(fileName);
            }
        }

        [HttpGet("{table}/{id}")]
        public ActionResult<string> Get(string table, string id)
        {
            string path = Path.Combine(_environment.WebRootPath, "uploads", table);
            if (!Directory.Exists(path))
                return BadRequest("Bad Path.");
            string filePath = Directory.GetFiles(path).ToList().Find(f => Path.GetFileName(f) == id.ToString());
            if (string.IsNullOrEmpty(filePath)) return BadRequest("Image Not Found.");

            byte[] b = System.IO.File.ReadAllBytes(filePath);
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var fileType);
            return File(b, fileType);
        }

        [HttpGet("{table}")]
        [AuthFilter]
        public ActionResult<string> RemoveFile(string table)
        {
            string path = Path.Combine(_environment.WebRootPath, "uploads", table);
            if (!Directory.Exists(path))
                return BadRequest("Bad Path.");
            List<string> filePath = Directory.GetFiles(path).ToList();
            var imageList = MyDataBase.GetImageList(table);
            var fileToDelete = filePath.FindAll(i => !imageList.Contains(Path.GetFileName(i)));

            fileToDelete.ForEach(f =>
            {
                System.IO.File.Delete(f);
            });
            return Ok(fileToDelete);
        }
    }
}
