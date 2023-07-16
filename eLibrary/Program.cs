using System.Security.Claims;
using eLibrary.Application.Services;
using eLibrary.Infrastructure.Context;
using eLibrary.Policy;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using static System.Net.WebRequestMethods;

namespace eLibrary;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddDbContext<LibraryDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
        });

        var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = builder.Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        builder.Services
            .AddAuthorization(options =>
            {
                options.AddPolicy(
                    "read:books",
                    policy => policy.Requirements.Add(
                        new HasScopeRequirement("read:books", domain)
                    )
                );
            });

        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eLibrary API", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });
            }
        );

        builder.Services.AddCors(c => c.AddPolicy("CorsPolicy",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithMethods("GET", "PUT", "DELETE", "POST",
                    "PATCH") //not really necessary when AllowAnyMethods is used.
        ));

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("CorsPolicy");

        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}