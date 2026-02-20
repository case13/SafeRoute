using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using SafeRoute.BlazorServer.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// 1) Auth/State
builder.Services.AddAuthenticationConfiguration();

builder.Services.AddAuthorizationConfiguration();

// 2) HttpClient(s)
builder.Services.AddApiHttpClient(builder.Configuration);

// 3) Services + ViewModels
builder.Services.AddFrontendServices();

builder.Services.AddMudServices();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
