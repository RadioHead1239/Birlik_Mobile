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

        public async Task<string?> GetTokenAsync()
        {
            if (CurrentUser == null)
                CurrentUser = await _db.GetSessionAsync();

            return CurrentUser?.Token;
        }


        public async Task<UsuarioInfoDTO?> ObtenerUsuarioLogueado()
        {
            if (CurrentUser != null)
            {
                return new UsuarioInfoDTO
                {
                    Id = CurrentUser.Id,
                    Correo = CurrentUser.Correo,
                    Nombre = CurrentUser.Nombre,
                    Rol = CurrentUser.Rol
                };
            }

            var session = await _db.GetSessionAsync();
            if (session == null)
                return null;

            CurrentUser = session;

            return new UsuarioInfoDTO
            {
                Id = session.Id,
                Correo = session.Correo,
                Nombre = session.Nombre,
                Rol = session.Rol
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
                Token = token 
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
