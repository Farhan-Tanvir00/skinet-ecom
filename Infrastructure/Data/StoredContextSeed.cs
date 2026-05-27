using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoredContextSeed
    {
        public static async Task SeedAsync(StoredContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/seed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if(products != null)
                {
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                };
            }
        }
    }
}
