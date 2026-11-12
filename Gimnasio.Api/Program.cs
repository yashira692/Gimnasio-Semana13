using Gimnasio.Datos.Models;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Negocio.Mapeos;
using Gimnasio.Negocio.Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext (la cadena de conexión está en User Secrets, no en el repositorio)
builder.Services.AddDbContext<GimnasioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GimnasioDB")));

// 2) Repositorios
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISocioRepository, SocioRepository>();

// 3) Servicios
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISocioService, SocioService>();

// 4) AutoMapper (un único MappingProfile en la capa de Negocio)
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                       // /openapi/v1.json
    app.UseSwaggerUI(o =>                   // /swagger
    {
        o.SwaggerEndpoint("/openapi/v1.json", "Gimnasio API v1");
        o.DocumentTitle = "Gimnasio API - SEMANA13";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
