
using BL.Interfaces;
using BL.Services;
using DAL.Repositories;
using Microsoft.OpenApi.Models;

namespace OpenTextService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add the factory itself:
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOpenTextRepository, OpenTextRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyOpenTextECM", Version = "v1" });

                c.AddSecurityDefinition("OpenTextApiKey", new OpenApiSecurityScheme
                {
                    Description = "OpenText Access Token using the Bearer scheme. \r\n\r\n" +
                                  "Enter 'Bearer' [space] and then your token.\r\n\r\n" +
                                  "Example: \"Bearer eyJ...\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "OpenTextApiKey"
                            },
                            Scheme = "Bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
