using Application;
using Application.Features;
using Application.Features.Chat;
using Application.Features.Shared;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

Assembly? applicationAssembly = typeof(DomainEvent).Assembly;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

// Add services to the container.
services
    .AddControllersWithViews()
    .AddApplicationPart(applicationAssembly);

services.AddSingleton<ConnectionStringProvider>();
services.AddSingleton<IConnectionFactory, ConnectionFactory>();
services.AddSingleton<IDbConnection, SqlConnection>();
services.AddSingleton<ChatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
