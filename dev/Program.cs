using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using FateExplorer.WPA.GameData;

namespace FateExplorer.WPA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();

            builder.Services.AddScoped<IGameDataService, DataServiceDSA5>();

            //await builder.Build().RunAsync();
            var host = builder.Build();

            var DataService = host.Services.GetRequiredService<IGameDataService>();
            await DataService.InitializeGameDataAsync();

            await host.RunAsync();
        }
    }
}
