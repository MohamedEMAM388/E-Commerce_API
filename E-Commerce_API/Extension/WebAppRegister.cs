using ECommerce.Domain.Contarct;
using ECommerce.Persistence.Data.Context;
using ECommerce.Persistence.IDentityData.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce_API.Extension
{
    public static class WebAppRegister
    {

        // extension methods to migrate database and seed data

        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app) {

            // create a scope to get the services
           await using var scope = app.Services.CreateAsyncScope();
           var dbcontext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any()) {

                await dbcontext.Database.MigrateAsync();
            }

            return app;
        }

        public static async Task<WebApplication> MigrateIDentityDataBaseAsync(this WebApplication app)
        {

            // create a scope to get the services
            await using var scope = app.Services.CreateAsyncScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<StoreIDentityContext>();

            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {

                await dbcontext.Database.MigrateAsync();
            }

            return app;
        }

        public static async Task<WebApplication> DataSeedAsync(this WebApplication app) { 
        
            await using var scope =  app.Services.CreateAsyncScope();
            var dataintializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("default");

            await dataintializer.InitializeDataAsync();

            return app;
        }

        public static async Task<WebApplication> IdentityDataSeedAsync(this WebApplication app)
        {

            await using var scope = app.Services.CreateAsyncScope();
            var dataintializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("identity");

            await dataintializer.InitializeDataAsync();

            return app;
        }
    }
}
