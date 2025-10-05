using Birlik_Mobile.Helpers;
using Birlik_Mobile.Services;

namespace Birlik_Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // 🚫 Comenta esta línea temporalmente
        // builder.Services.AddBirlikServices();

        // ✅ Inyectamos directamente el HttpClient con ApiKey
        builder.Services.AddScoped(sp =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.BaseUrl)
            };

            // 🔑 Agregamos manualmente el header
            if (!client.DefaultRequestHeaders.Contains("x-api-key"))
                client.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);

            Console.WriteLine($"✅ HttpClient directo configurado con {ApiConstants.BaseUrl} y ApiKey {ApiConstants.ApiKey}");
            return client;
        });

        // ✅ Registramos el ApiService directamente
        builder.Services.AddScoped<ApiService>();

        // Blazor WebView
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        return builder.Build();
    }
}
