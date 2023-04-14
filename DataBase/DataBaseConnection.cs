using System.Data.SQLite;

namespace EmergencySituations.DataBase
{
    public class DataBaseConnection : IDisposable
    {
        private SQLiteConnection _connection;
        public SQLiteConnection Connection => _connection;
        public DataBaseConnection()
        {
            _connection = new SQLiteConnection(MyDataBase.SqlConfig);
            _connection.Open();
        }
        List<SQLiteCommand> _commands = new List<SQLiteCommand>();
        public SQLiteCommand CreateCommand(string q, Dictionary<string, string> args = null)
        {
            SQLiteCommand cmd = new SQLiteCommand(q, _connection);
            if(args != null)
                foreach (var a in args)
                {
                    cmd.Parameters.AddWithValue("@" + a.Key, a.Value);
                }
            _commands.Add(cmd);
            return cmd;
        }

        ~DataBaseConnection()
        {
            Dispose();
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            _commands.ForEach(cmd => cmd.Dispose());
            _commands.Clear();
            _connection.Close();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
