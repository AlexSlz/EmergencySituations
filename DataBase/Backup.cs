using EmergencySituations.Other.Model;

namespace EmergencySituations.DataBase
{
    public static class Backup
    {
        private static DateTime _lastBackUp = DateTime.MinValue;
        public static MyRequestResult CreateBackup(string rootFolder)
        {
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
