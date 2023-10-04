using Application;
using Application.Features.Authentication;
using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using Application.Features.Authentication.Validators;
using Application.Features.Conversations;
using Application.Features.Conversations.Dtos;
using Application.Features.Conversations.Interfaces;
using Application.Features.Conversations.Validators;
using Application.Features.Chatter;
using Application.Features.Chatter.Interfaces;
using Application.Features.Conversation.Dtos;
using Application.Features.Conversation.Validators;
using Application.Features.Shared;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using Web;

Assembly? applicationAssembly = typeof(DomainEvent).Assembly;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

// Add services to the container.
services.AddHealthChecks();

services
    .AddControllersWithViews()
    .AddApplicationPart(applicationAssembly);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

services.AddSingleton<IRegisterCommand, RegisterCommand>();
services.AddSingleton<ISecretHasher, SecretHasher>();
services.AddSingleton<IConnectionFactory, ConnectionFactory>();
services.AddSingleton<IDbConnection, SqlConnection>();
services.AddSingleton<IConversationEventHandlerFactory, ConversationEventHandlerFactory>();
services.AddSingleton<IConversationRepository, ConversationRepository>();
services.AddSingleton<IChatterRepository, ChatterRepository>();

services.AddSingleton<IValidator<StartConversationModel>, StartConversationModelValidator>();
services.AddSingleton<IValidator<PostMessageModel>, PostMessageModelValidator>();
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

SqlMapper.AddTypeHandler(new ChatterIdTypeHandler());
SqlMapper.AddTypeHandler(new MessageIdTypeHandler());
SqlMapper.AddTypeHandler(new ConversationIdTypeHandler());

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
