using Birlik_Mobile.Helpers;
using Birlik_Mobile.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Birlik_Mobile.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Método de extensión para registrar todos los servicios e inyectar dependencias.
        /// </summary>
        public static IServiceCollection AddBirlikServices(this IServiceCollection services)
        {
            // 🔹 Configurar HttpClient con ApiKey y entorno dinámico
            services.AddScoped(sp =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(ApiConstants.BaseUrl)
                };
                client.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);
                return client;
            });

            // 🔹 Registrar servicios de dominio
            services.AddScoped<ApiService>();
            // 🧩 Aquí puedes agregar más servicios en el futuro:
            // services.AddScoped<AuthService>();
            // services.AddScoped<ClienteService>();
            // services.AddScoped<PolizaService>();

            return services;
        }
    }
}
