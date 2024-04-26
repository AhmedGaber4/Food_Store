using Microsoft.Extensions.Logging;
using Store.Data;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDBContext Context,ILoggerFactory loggerFactory)
        {
            try
            {
                if (Context.ProductBrands != null && !Context.ProductBrands.Any())
                {
                     var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if(brands is not null)
                    {
                     
                            await Context.ProductBrands.AddRangeAsync(brands);
                        await Context.SaveChangesAsync();
                    }
                }
                if (Context.ProductTypes != null && !Context.ProductTypes.Any())
                {
                    var TypesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    if (types is not null)
                    {
                        
                            await Context.ProductTypes.AddRangeAsync(types);
                        await Context.SaveChangesAsync();
                    }
                }
                if (Context.Products != null && !Context.Products.Any())
                {
                    var ProductsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    if (products is not null)
                    {

                        await Context.Products.AddRangeAsync(products);
                        await Context.SaveChangesAsync();
                    }
                }
                if (Context.DeliveryMethods != null && !Context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                    if (DeliveryMethods is not null)
                    {

                        await Context.DeliveryMethods.AddRangeAsync(DeliveryMethods);
                        await Context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);

            }

        }
    }
}
