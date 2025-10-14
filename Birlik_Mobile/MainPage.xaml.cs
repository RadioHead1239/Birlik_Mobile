using Microsoft.AspNetCore.Components.WebView.Maui;

namespace Birlik_Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += (s, e) =>
            {
                Console.WriteLine("✅ BlazorWebView inicializado correctamente");
            };
        }
    }
}
