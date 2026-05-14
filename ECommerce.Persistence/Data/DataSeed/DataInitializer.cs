using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Product;
using ECommerce.Persistence.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeDataAsync()
        {
            try
            {

                // check if tables have data or not, if not then seed data
                var hasbrand = await _dbContext.ProductBrands.AnyAsync();
                var hastype = await _dbContext.ProductTypes.AnyAsync();
                var hasproduct = await _dbContext.Products.AnyAsync();

                if (hasbrand && hastype && hasproduct)
                    return;

                if (!hasbrand)
                {

                   await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }
                if (!hastype)
                {

                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }

                await _dbContext.SaveChangesAsync();
                if (!hasproduct)
                {
                   await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                   await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"Error initializing data: {ex.Message}");
            }
        }

        private async Task SeedDataFromJsonAsync<T, TKey>(string filename, DbSet<T> dbset) where T : BaseEbtity<TKey> {
            //D:\.Net_Intern\ECommerce.Persistence\Data\DataSeed\JsonFiles\
            var filepath = @"..\ECommerce.Persistence\Data\DataSeed\JsonFiles\" + filename;
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("Json File Not Found", filepath);
            }

            try {

                // open stream to read json file

                var datastream = File.OpenRead(filepath);

                var data = await JsonSerializer.DeserializeAsync<List<T>>(datastream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (data is not null)
                {

                   await dbset.AddRangeAsync(data);
                }
            } catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"Error seeding data from {filename}: {ex.Message}");
            }

        }
    } 

}
