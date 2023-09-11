using Application;
using Application.Features.Authentication;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Chat;
using Application.Features.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

Assembly? applicationAssembly = typeof(DomainEvent).Assembly;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

// Add services to the container.
services.AddHealthChecks();

services
    .AddControllersWithViews()
    .AddApplicationPart(applicationAssembly);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

services.AddSingleton<ISecretHasher, SecretHasher>();
services.AddSingleton<IConnectionFactory, ConnectionFactory>();
services.AddSingleton<IDbConnection, SqlConnection>();
services.AddSingleton<IChatEventHandlerFactory, ChatEventHandlerFactory>();
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

app.MapHealthChecks("/health");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


public partial class Program { }
