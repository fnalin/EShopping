using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        bool checkProducts = productCollection.Find(_ => true).Any();
        string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data", "Seed", "products.json");
        
        if (!checkProducts)
        {
            var productsData = File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products is not null)
            {
                foreach (var item in products)
                {
                    productCollection.InsertOneAsync(item);
                }
            }
        }
    }
}