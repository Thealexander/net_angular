using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//anexamos el archivo de ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


//modelo de tokens para autenticar
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6aB8wU7hbzjHcxv17bdhcG8uZTq0hjJI"));

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});
// Configurar políticas CORS
var mCors = "CorsRules";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(mCors, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddOcelot();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOcelot().Wait();
//app.UseAuthorization();

//app.MapControllers();

app.Run();
