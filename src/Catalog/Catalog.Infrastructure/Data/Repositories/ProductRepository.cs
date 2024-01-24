using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _ctx;

    public ProductRepository(ICatalogContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _ctx.Products.Find(p => true).ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _ctx.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _ctx.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrand(string brand)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, brand);
        return await _ctx.Products.Find(filter).ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _ctx.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _ctx.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        var deleteResult = await _ctx.Products.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }


    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _ctx.Brands.Find(b => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _ctx.Types.Find(p => true).ToListAsync();
    }
} 