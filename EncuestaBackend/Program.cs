using Data.Context;
using Data.Repositories.Implementaciones;
using Data.Repositories.Interfaces;
using EncuestaBackend.Utils.Fabrica;
using Logica.Services.Implementaciones;
using Logica.Services.Interfaces;
using Logica.Utils;
using Presentacion.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EncuestaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración JWT
            var claveSecreta = builder.Configuration["Jwt:ClaveSecreta"] ?? throw new ArgumentNullException("Jwt:ClaveSecreta no configurada");
            var emisor = builder.Configuration["Jwt:Emisor"] ?? throw new ArgumentNullException("Jwt:Emisor no configurado");
            var audiencia = builder.Configuration["Jwt:Audiencia"] ?? throw new ArgumentNullException("Jwt:Audiencia no configurada");

            var key = Encoding.ASCII.GetBytes(claveSecreta);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = emisor,
                    ValidAudience = audiencia,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddHttpContextAccessor();

            // Configuración de la base de datos
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Cadena de conexión no configurada");
            builder.Services.AddDbContext<EncuestaDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registro de servicios
            builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
            builder.Services.AddScoped<IRespuestaServicio, RespuestaServicio>();
            builder.Services.AddScoped<IRespuestaRepositorio, RespuestaRepositorio>();
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<IEncuestaServicio, EncuestaServicio>();
            builder.Services.AddScoped<IFabricaPregunta, FabricaPregunta>();
            builder.Services.AddScoped<IEncuestaRepositorio, EncuestaRepositorio>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // 🔴🔴🔴 SOLUCIÓN CLAVE: Registrar controladores desde Presentación 🔴🔴🔴
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentacion.Controllers.EncuestaControlador).Assembly)
                .AddControllersAsServices();

            // Configuración Swagger mejorada
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Encuesta API",
                    Version = "v1",
                    Description = "API para el sistema de encuestas",
                    Contact = new OpenApiContact { Name = "Equipo de Desarrollo", Email = "dev@encuesta.com" }
                });

                // Configuración JWT en Swagger
                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Ingrese 'Bearer' seguido de su token JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } });

                // 🔴🔴🔴 IMPORTANTE: Forzar inclusión de todos los endpoints
                c.DocInclusionPredicate((docName, apiDesc) => true);

                // Incluir comentarios XML de ambos proyectos
                try
                {
                    var presentationXml = Path.Combine(AppContext.BaseDirectory, "Presentacion.xml");
                    var backendXml = Path.Combine(AppContext.BaseDirectory, "EncuestaBackend.xml");

                    if (File.Exists(presentationXml)) c.IncludeXmlComments(presentationXml);
                    if (File.Exists(backendXml)) c.IncludeXmlComments(backendXml);
                }
                catch
                {
                    // Opcional: Loggear si hay error al cargar los XML
                }
            });

            // Configuración CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Pipeline de configuración
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Encuesta API v1");
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("PermitirFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}