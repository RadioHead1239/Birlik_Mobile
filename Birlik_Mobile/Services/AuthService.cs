using Birlik_Mobile.Models.Auth;
using Birlik_Mobile.Services.Storage;

namespace Birlik_Mobile.Services
{
    public class AuthService
    {
        private readonly LocalDatabase _db;
        public UserSession? CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser != null;

        public AuthService(LocalDatabase db)
        {
            _db = db;
        }

        public async Task InitializeAsync()
        {
            CurrentUser = await _db.GetSessionAsync();
        }

        public async Task SetUserAsync(UsuarioInfoDTO user)
        {
            var session = new UserSession
            {
                Usuario = user.Usuario,
                Rol = user.Rol
            };
            await _db.SaveSessionAsync(session);
            CurrentUser = session;
        }

        public async Task LogoutAsync()
        {
            await _db.ClearSessionAsync();
            CurrentUser = null;
        }
    }
}
