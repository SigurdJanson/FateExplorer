using FateExplorer;
using FateExplorer.Calendar;
using FateExplorer.GameData;
using FateExplorer.Shared;
using FateExplorer.Shared.ClientSideStorage;
using FateExplorer.ViewModel;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Net.Http;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Services
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Services of Fate Explorer
builder.Services
    .AddScoped<IClientSideStorage, CookieStorage>()
    .AddScoped<IGameDataService, DataServiceDSA5>()
    .AddScoped<ITheHeroViMo, TheHeroViMo>()
    .AddScoped<IDateOfPlay, DateOfPlayM>()
    .AddSingleton<AppSettings>();
builder.Services.AddScoped<IRollHandlerViMo, RollHandlerViMo>();

var host = builder.Build();

// Fate Explorer Setup
var DataService = host.Services.GetRequiredService<IGameDataService>();
await DataService.InitializeGameDataAsync();

var RollHandlerService = host.Services.GetRequiredService<IRollHandlerViMo>();
await RollHandlerService.ReadRollMappingsAsync();
RollHandlerService.RegisterChecks();

// Run
await host.RunAsync();
