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

            services.AddScoped<ApiService>();

            services.AddSingleton<AuthService>();

         

            services.AddSingleton<LocalDatabase>(sp =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "birlik_session.db3");
                return new LocalDatabase(dbPath);
            });

            services.AddSingleton<AuthService>();

            // services.AddScoped<ClienteService>();
            // services.AddScoped<PolizaService>();
            // services.AddScoped<FacturaService>();

            return services;
        }
    }
}
