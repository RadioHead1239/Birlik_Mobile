using Birlik_Mobile.Helpers;
using Birlik_Mobile.Services;
using Birlik_Mobile.Services.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Birlik_Mobile.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra todos los servicios principales de la aplicación (HTTP, API, Auth, DB local, etc.)
        /// </summary>
        public static IServiceCollection AddBirlikServices(this IServiceCollection services)
        {
            // HttpClient simple con header x-api-key
            services.AddScoped(sp =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(ApiConstants.BaseUrl)
                };

                if (!client.DefaultRequestHeaders.Contains("x-api-key"))
                    client.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);

                Console.WriteLine($"✅ HttpClient configurado con BaseUrl={ApiConstants.BaseUrl}");

                return client;
            });

            // Servicios de dominio
            services.AddScoped<ApiService>();
            services.AddSingleton<NotificationService>();

            // Base local y auth como singletons (una única instancia durante el ciclo de vida de la app)
            services.AddSingleton<LocalDatabase>();   // usa el ctor sin parámetros que ya tienes
            services.AddSingleton<AuthService>();

            // (Descomentarlos cuando los implementes)
            // services.AddScoped<ClienteService>();
            // services.AddScoped<PolizaService>();
            // services.AddScoped<FacturaService>();

            return services;
        }
    }
}
