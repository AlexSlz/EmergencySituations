using EmergencySituations.Auth;
using EmergencySituations.DataBase;
using EmergencySituations.Other;
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

        [HttpGet("backup/create")]
        [AuthFilter]
        public ActionResult<string> CreateBackup()
        {
            var res = Backup.CreateBackup(_environment.WebRootPath);
            if (res.isError)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [HttpGet("backup/list")]
        [AuthFilter]
        public ActionResult<string> GetBackupList()
        {
            return Ok(Backup.GetList(_environment.WebRootPath));
        }

        [HttpGet("backup/{fileName}")]
        [AuthFilter]
        public ActionResult<string> LoadBackup(string fileName)
        {
            if(fileName == "")
                return BadRequest();
            var res = Backup.LoadFile(_environment.WebRootPath, fileName);
            if (res.isError)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [HttpGet("backup/{fileName}/delete")]
        [AuthFilter]
        public ActionResult<string> DeleteBackup(string fileName)
        {
            if (fileName == "")
                return BadRequest();
            var res = Backup.DeleteFile(_environment.WebRootPath, fileName);
            if (res.isError)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }


        [HttpPost("{table}/{id}")]
        [AuthFilter]
        public ActionResult<string> UploadFile([FromForm] IFormFile file, string table, string id)
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
        public ActionResult<string> GetFile(string table, string id)
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

        [HttpGet("report")]
        [AuthFilter]
        public ActionResult<string> GetReport(int year, int month = 0)
        {
            if (!DateTime.TryParse(string.Format("1/1/{0}", year), out var dateTime))
            {
                return BadRequest("Error Year");
            }
            year = dateTime.Year;
            string path = Path.Combine(_environment.WebRootPath, "reports");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, "temp.docx");
            if (!FileManager.CreateDoc(filePath, year, month))
            {
                return NotFound();
            }

            byte[] b = System.IO.File.ReadAllBytes(filePath);
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var fileType);

            var resultName = $"Звіт_{year}.docx";
            if (month > 0)
                resultName = $"Звіт_{month}_{year}.docx";

            return File(b, fileType, resultName);
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
