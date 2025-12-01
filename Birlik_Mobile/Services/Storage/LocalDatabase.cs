using SQLite;

namespace Birlik_Mobile.Services.Storage
{
    public class LocalDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public LocalDatabase()
        {
            // Ruta donde se guardará la BD
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "birlik_session.db3");

            // Crear la conexión
            _db = new SQLiteAsyncConnection(dbPath);

            // Crear la tabla al iniciar
            _db.CreateTableAsync<UserSession>().Wait();
        }

        // Guarda o reemplaza la sesión
        public Task<int> SaveSessionAsync(UserSession session)
        {
            return _db.InsertOrReplaceAsync(session);
        }

        // Obtiene la sesión actual
        public Task<UserSession?> GetSessionAsync()
        {
            return _db.Table<UserSession>().FirstOrDefaultAsync();
        }

        // Borra toda la sesión
        public Task<int> ClearSessionAsync()
        {
            return _db.DeleteAllAsync<UserSession>();
        }
    }

    [Table("UserSession")]
    public class UserSession
    {
        [PrimaryKey, AutoIncrement]
        public int SessionId { get; set; }

        public int Id { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int IdCliente { get; set; }
    }
}
