using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _ctx;

    public ProductRepository(ICatalogContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Pagination<Product>> GetAllProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            filter &= builder.Regex(x => x.Name, new BsonRegularExpression(catalogSpecParams.Search));
        }
        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            filter &= builder.Eq(x => x.Brands.Id, catalogSpecParams.BrandId);
        }
        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            filter &= builder.Eq(x => x.Types.Id, catalogSpecParams.TypeId);
        }
        
        var dataResponse = new Pagination<Product>
        {
            PageSize = catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams.PageIndex,
            Count = await _ctx.Products.CountDocumentsAsync(p=>true)
        };

        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            dataResponse.Data = await DataFilter(catalogSpecParams, filter);
        }
        else
        {
            dataResponse.Data = await _ctx
                .Products
                .Find(filter)
                .Sort(Builders<Product>.Sort.Ascending("Name"))
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }

        return dataResponse;
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        var data =  _ctx
            .Products
            .Find(filter);
        switch (catalogSpecParams.Sort)
        {
             case "priceAsc":
                 data.Sort(Builders<Product>.Sort.Ascending("Price"));
                 break;
            case "priceDesc":
                data.Sort(Builders<Product>.Sort.Descending("Price"));
                break;
            default:
                data.Sort(Builders<Product>.Sort.Descending("Name"));
                break;
        }

        return await data
            .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();

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