using SQLite;

namespace Birlik_Mobile.Services.Storage
{
    public class LocalDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public LocalDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "birlik_session.db3");
            _db = new SQLiteAsyncConnection(dbPath);

            _db.CreateTableAsync<UserSession>().Wait();
            Console.WriteLine($"📁 Ruta de BD local: {dbPath}");

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
        public int LocalId { get; set; }  // <-- PK REAL

        public int Id { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }



}
