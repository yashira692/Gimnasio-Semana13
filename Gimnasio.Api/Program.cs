using System.Text;
using Gimnasio.Datos.Models;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Negocio.Mapeos;
using Gimnasio.Negocio.Seguridad;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext (la cadena de conexión está en User Secrets, no en el repositorio)
builder.Services.AddDbContext<GimnasioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GimnasioDB")));

// 2) Repositorios
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISocioRepository, SocioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// 3) Servicios
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISocioService, SocioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 4) AutoMapper (un único MappingProfile en la capa de Negocio)
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

// 5) JWT: emisor, audiencia y minutos en appsettings.json; la Clave en User Secrets
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOpciones>()
          ?? throw new InvalidOperationException("Falta la sección Jwt en la configuración.");
if (string.IsNullOrWhiteSpace(jwt.Clave) || jwt.Clave.Length < 32)
    throw new InvalidOperationException("Jwt:Clave debe tener al menos 32 caracteres (configúrala con dotnet user-secrets).");
builder.Services.AddSingleton(jwt);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // conserva los nombres cortos: sub, email, role
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Emisor,
            ValidateAudience = true,
            ValidAudience = jwt.Audiencia,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            NameClaimType = "email",
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        // Devuelve tildes y apóstrofes legibles en el JSON (sin á, ')
        o.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping);

// 6) OpenAPI con esquema Bearer -> botón "Authorize" en Swagger
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Pega solo el token del login (sin la palabra Bearer)."
        };
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                       // /openapi/v1.json
    app.UseSwaggerUI(o =>                   // /swagger
    {
        o.SwaggerEndpoint("/openapi/v1.json", "Gimnasio API v1");
        o.DocumentTitle = "Gimnasio API - SEMANA14 (JWT)";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();   // primero: ¿quién eres? (lee y valida el token)
app.UseAuthorization();    // después: ¿qué puedes hacer? ([Authorize], roles)
app.MapControllers();

app.Run();
