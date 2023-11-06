using ControlAcceso.Core.Application;
using ControlAcceso.Core.Entities;
using ControlAcceso.Core.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.Extensions.Logging; // Agregar esta directiva
using System.Reflection;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Servicios de SQL Server & CORS
builder.Services.AddDbContext<SeguridadContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cn")));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configuración de Identity
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<SeguridadContexto>()
    .AddSignInManager<SignInManager<Usuario>>();

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


//AutoMapper
builder.Services.AddAutoMapper(typeof(Register.UsuarioRegisterHandler));

var mCors = "CorsRules";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: mCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod();

    });
});

builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Register>());

// Aprende más sobre la configuración de Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication(); // Agregar autenticación

app.MapControllers();

using (var contexto = app.Services.CreateScope())
{
    var services = contexto.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var contextoEF = services.GetRequiredService<SeguridadContexto>();

        SeguridadData.InsertarUsuario(contextoEF, userManager).Wait();
    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "error al registrar usuario");
    }
}

app.Run();
