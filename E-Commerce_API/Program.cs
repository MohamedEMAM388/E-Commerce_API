
using E_Commerce_API.CustomMiddelWares;
using E_Commerce_API.Extension;
using E_Commerce_API.Factories;
using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistence.Data.Context;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.IDentityData.Contexts;
using ECommerce.Persistence.IDentityData.DataSeed;
using ECommerce.Persistence.Repositories;
using ECommerce.Services;
using ECommerce.Services.Profiles;
using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.ECommerce", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddDbContext<StoreDbContext>(options => {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(opt => {

                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });

            builder.Services.AddKeyedScoped<IDataInitializer , DataInitializer>("default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataIntializer>("identity");

            builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);

            builder.Services.AddScoped<IunitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductServies, ProductServies>();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddScoped<IBasketServies, BasketServies>();

            builder.Services.AddScoped<ICacheRepository, CacheRepository>();

            builder.Services.AddScoped<ICacheServies, CacheServies>();

            // Custom Validation Response
            builder.Services.Configure<ApiBehaviorOptions>(options => {

                // InvalidModelStateResponseFactory => is a delegate take actionContext and return IActionResult
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;

            });

            builder.Services.AddDbContext<StoreIDentityContext>(options => { 
            
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddIdentityCore<ApplicationUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<StoreIDentityContext>();

            builder.Services.AddScoped<IAuthenticationServies, AuthenticationServies>();

            // validate on token in each request

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters() { 
                                   
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:key"]!))

                };
            
            });

            builder.Services.AddScoped<IOrderServies, OrderServies>();
            builder.Services.AddScoped<IPaymentServies, PaymentServies>();

            // Pipelines [MiddleWares]

            var app = builder.Build();

            await app.MigrateDataBaseAsync(); 
            await app.MigrateIDentityDataBaseAsync();
            await app.DataSeedAsync();
            await app.IdentityDataSeedAsync();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlerMiddelWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => {

                    options.DisplayRequestDuration();
                    options.EnableFilter();
                    options.DocExpansion(DocExpansion.None);
                });
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
