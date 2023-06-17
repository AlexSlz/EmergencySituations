using EmergencySituations.DataBase.Model;
using EmergencySituations.Other.Model;

namespace EmergencySituations.DataBase
{
    public static class Backup
    {
        private static DateTime _lastBackUp = DateTime.MinValue;
        private static string _lastBackUpName = string.Empty;
        public static MyRequestResult CreateBackup(string rootFolder)
        {
            _lastBackUpName = string.Empty;
            var date = DateTime.Now;
            if (_lastBackUp > date)
                return new MyRequestResult("Занадто швидко.", true);
            var folder = GetFolder(rootFolder);
            if (File.Exists(MyDataBase.MyDBFile))
            {
                var ex = new FileInfo(MyDataBase.MyDBFile).Extension;
                var newFileName = $"{date.ToString("yyyy-MM-dd(HH-mm-ss)")}{ex}";
                File.Copy(MyDataBase.MyDBFile, Path.Combine(folder, newFileName));
                _lastBackUp = date.AddMinutes(1);
                _lastBackUpName = newFileName;
                return new MyRequestResult($"Файл створено, {newFileName}.");
            }
            return new MyRequestResult("Файл бази даних не існує.", true);
        }

        public static string[] GetList(string rootFolder)
        {
            var folder = GetFolder(rootFolder);
            return new DirectoryInfo(folder).GetFiles().Select(i => i.Name).ToArray();
        }

        public static MyRequestResult LoadFile(string rootFolder, string fileName)
        {
            var folder = GetFolder(rootFolder);
            var file = Path.Combine(folder, fileName);
            if (File.Exists(file))
            {
                File.Copy(file, MyDataBase.MyDBFile, true);
                return new MyRequestResult($"Файл завантажено.");
            }
            return new MyRequestResult($"Файла не існує.");
        }

        public static MyRequestResult UploadFile(string rootFolder, IFormFile file)
        {
            CreateBackup(rootFolder);
            var folder = GetFolder(rootFolder);

            using (FileStream fileStream = System.IO.File.Create(Path.Combine(folder, file.FileName)))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            LoadFile(rootFolder, file.FileName);
            bool error = false;
            try
            {
                MyDataBase.Select<Users>();
                MyDataBase.Select<Emergency>();
            }
            catch (Exception)
            {
                LoadFile(rootFolder, _lastBackUpName);
                DeleteFile(rootFolder, file.FileName);
                error = true;
            }
            DeleteFile(rootFolder, _lastBackUpName);
            _lastBackUpName = string.Empty;
            if (!error)
                return new MyRequestResult("Файл завантажено.");
            
            return new MyRequestResult("Сталася помилка.", true);
        }

        public static MyRequestResult DeleteFile(string rootFolder, string fileName)
        {
            var folder = GetFolder(rootFolder);
            var file = Path.Combine(folder, fileName);
            if (File.Exists(file))
            {
                File.Delete(file);

                return new MyRequestResult($"Файл видалено.");
            }
            return new MyRequestResult($"Файла не існує.");
        }


        private static string GetFolder(string rootFolder)
        {
            var folder = Path.Combine(rootFolder, "backup");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }
}
