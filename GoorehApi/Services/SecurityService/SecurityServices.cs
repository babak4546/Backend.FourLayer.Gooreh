using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehInfrastructure.DbContextes;
using GoorehInfrastructure.Repositorys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.Services.SecurityService
{
    public class SecurityServices
    {
        public static void AddServices(WebApplicationBuilder builder)
        {
            
            builder.Services.AddScoped<UserNoteRepository>();
            builder.Services.AddScoped<IGenericRepository<UserContact>,GenericRepository<UserContact>>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            builder.Services.AddDbContext<GoorehDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("GoorehDb"));
            });
            builder.Services.AddCors(options
                => options.AddDefaultPolicy
                (builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]??""))
                };
            });

            builder.Services.AddAuthorization();
        }
        public static void UseServises(WebApplication app)
        {
            
            app.UseCors();




        }

    }
}
