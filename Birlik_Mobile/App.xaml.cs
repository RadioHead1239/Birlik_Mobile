using Birlik_Mobile.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics; // Agrega esta línea para importar Colors
using Microsoft.Maui; // Agrega esta línea para importar Thickness

namespace Birlik_Mobile
{
    // Solución: Usar el nombre completo para evitar ambigüedad
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly AuthService _authService;

        public App(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            // 🔹 Pantalla temporal mientras carga
            MainPage = new ContentPage
            {
                BackgroundColor = Colors.White,
                Content = new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new ActivityIndicator
                        {
                            IsRunning = true,
                            Color = Colors.Blue,
                            WidthRequest = 40,
                            HeightRequest = 40
                        },
                        new Label
                        {
                            Text = "Cargando Birlik...",
                            TextColor = Colors.Black,
                            FontSize = 18,
                            Margin = new Thickness(0,15,0,0)
                        }
                    }
                }
            };

            // 🔹 Ejecuta inicialización sin bloquear la UI
            _ = InitializeAppAsync();
        }

        private async Task InitializeAppAsync()
        {
            try
            {
                Console.WriteLine("🟡 Iniciando InitializeAppAsync...");

                await Task.Delay(300);

                Console.WriteLine("🔹 Inicializando AuthService...");
                await _authService.InitializeAsync();
                Console.WriteLine("✅ AuthService inicializado correctamente");

                // 🚀 Carga la página principal
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Console.WriteLine("🚀 Cambiando a MainPage...");
                    MainPage = new MainPage();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al iniciar: {ex}");
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MainPage = new ContentPage
                    {
                        Content = new Label
                        {
                            Text = $"Error al iniciar: {ex.Message}",
                            TextColor = Colors.Red,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        }
                    };
                });
            }
        }
    }
}
