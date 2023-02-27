using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PortalBlazor.Client;
using PortalBlazor.Client.Auth;
using PortalBlazor.Core.Services.JobJourney;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<IJobJourneyService, JobJourneyService>();
builder.Services.AddScoped<IAuthorizeService, AuthStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());

builder.Services.AddMudServices();
await builder.Build().RunAsync();