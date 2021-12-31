using FateExplorer.WPA.GameData;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
//using Microsoft.JSInterop;

namespace FateExplorer.WPA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Services
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();
            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            builder.Services.AddScoped<IGameDataService, DataServiceDSA5>();

            var host = builder.Build();

            // Fate Explorer
            var DataService = host.Services.GetRequiredService<IGameDataService>();
            await DataService.InitializeGameDataAsync();

            // Run
            await host.RunAsync();
        }
    }
}
