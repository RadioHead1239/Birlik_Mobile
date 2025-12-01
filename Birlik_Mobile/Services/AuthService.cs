using Birlik_Mobile.Models.Auth;
using Birlik_Mobile.Services.Storage;

namespace Birlik_Mobile.Services
{
    public class AuthService
    {
        private readonly LocalDatabase _db;
        public UserSession? CurrentUser { get; private set; }

        // ✅ Usa 'is not null' en lugar de '!= null'
        public bool IsLoggedIn => CurrentUser is not null;

        public AuthService(LocalDatabase db)
        {
            _db = db;
        }

        public async Task<string?> GetTokenAsync()
        {
            // ✅ Usa 'is null' en lugar de '== null'
            if (CurrentUser is null)
                CurrentUser = await _db.GetSessionAsync();

            return CurrentUser?.Token;
        }

        public async Task<UsuarioInfoDTO?> ObtenerUsuarioLogueado()
        {
            // ✅ Usa 'is not null' en lugar de '!= null'
            if (CurrentUser is not null)
            {
                return new UsuarioInfoDTO
                {
                    Id = CurrentUser.Id,
                    Correo = CurrentUser.Correo,
                    Nombre = CurrentUser.Nombre,
                    Rol = CurrentUser.Rol,
                    IdCliente = CurrentUser.IdCliente
                };
            }

            var session = await _db.GetSessionAsync();

            // ✅ Usa 'is null' en lugar de '== null'
            if (session is null)
                return null;

            CurrentUser = session;

            return new UsuarioInfoDTO
            {
                Id = session.Id,
                Correo = session.Correo,
                Nombre = session.Nombre,
                Rol = session.Rol,
                IdCliente = session.IdCliente
            };
        }

        public async Task InitializeAsync()
        {
            try
            {
                CurrentUser = await _db.GetSessionAsync();
            }
            catch (Exception)
            {
                CurrentUser = null;
            }
        }

        public async Task SetUserAsync(UsuarioInfoDTO user, string token)
        {
            var session = new UserSession
            {
                Id = user.Id,
                Correo = user.Correo,
                Nombre = user.Nombre,
                Rol = user.Rol,
                Token = token,
                IdCliente = user.IdCliente
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