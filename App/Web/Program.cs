using Application;
using Application.Features;
using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Chat;
using Application.Features.Shared;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
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

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

services.AddSingleton<ISecretHasher, SecretHasher>();
services.AddSingleton<IConnectionFactory, ConnectionFactory>();
services.AddSingleton<IDbConnection, SqlConnection>();
services.AddSingleton<ChatRepository>();
services.AddSingleton<IValidator<UserRegisterModel>, RegisterValidator>();
services.AddSingleton<IValidator<UserLoginModel>, LoginValidator>();
services.AddSingleton<IUserRepository, UserRepository>();
services.AddSingleton<ISignInManager, SignInManager>();


services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/Authentication/Login";
            options.LogoutPath = "/Authentication/Logout";
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    builder.Configuration["ConnectionStringName"] = "Local";
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


if (app.Environment.IsDevelopment())
{
    IContainerService container = new Builder()
        .UseContainer()
        .DeleteIfExists(force: true)
        .UseImage("sqlsampleserver:1.0.0")
        .WithName("SqlServerSampleContainer")
        .ExposePort(1433)
        .WithEnvironment($"password={builder.Configuration["DockerSamplePassword"]}")
        .WaitForPort("1433/tcp", 10)
        .Build()
        .Start();
    builder.Configuration["ConnectionStringName"] = "DockerSample";

    app.Lifetime.ApplicationStopping.Register(() =>
    {
        container.Dispose();
    });
    
}

app.Run();


public partial class Program { }
