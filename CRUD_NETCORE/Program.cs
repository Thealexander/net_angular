using microservicios.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using microservicios.Core.ContextMongoDB;
using microservicios.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Estas dos líneas son para SQL Server
// builder.Services.AddDbContext<CrudContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("conecction")));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configurar la conexión a MongoDB
builder.Services.Configure<MongoSettings>(options =>
{
    options.ConnectionString = builder.Configuration.GetSection("MongoDb:ConnectionString").Value;
    options.Database = builder.Configuration.GetSection("MongoDb:Database").Value;
});
builder.Services.AddSingleton<MongoSettings>();
builder.Services.AddTransient<IAutorContext, AutorContext>();
builder.Services.AddTransient<IAutorRepository, AutorRepository>();
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

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

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(mCors);
app.UseAuthorization();
app.MapControllers();
app.Run();
