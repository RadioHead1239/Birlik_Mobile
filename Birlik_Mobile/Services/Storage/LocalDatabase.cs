using SQLite;

namespace Birlik_Mobile.Services.Storage
{
    public class LocalDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public LocalDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<UserSession>().Wait();
        }

        public Task<int> SaveSessionAsync(UserSession session)
        {
            return _db.InsertOrReplaceAsync(session);
        }

        public Task<UserSession?> GetSessionAsync()
        {
            return _db.Table<UserSession>().FirstOrDefaultAsync();
        }

        public Task<int> ClearSessionAsync()
        {
            return _db.DeleteAllAsync<UserSession>();
        }
    }

    [Table("UserSession")]
    public class UserSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Usuario { get; set; } = "";
        public string Rol { get; set; } = "";
        public string Token { get; set; } = ""; 
    }
}
