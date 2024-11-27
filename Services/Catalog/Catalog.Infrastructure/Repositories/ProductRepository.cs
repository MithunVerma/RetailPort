
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        public ICatalogContext _catalogContext { get; }
        public ProductRepository(ICatalogContext catalogContext)
        { 
            _catalogContext = catalogContext;
        }
        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext.Products.Find(p  => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Pagination<Product>> GetProducts(CatalagSpecParams catalagSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if(!string.IsNullOrEmpty(catalagSpecParams.Search))
            {
                filter = filter & builder.Where(p=> p.Name.ToLower().Contains(catalagSpecParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalagSpecParams.BrandId))
            {
                var brandFilter = filter & builder.Eq(p => p.Brands.Id, catalagSpecParams.BrandId);
            }
            if (!string.IsNullOrEmpty(catalagSpecParams.TypeId))
            {
                var typeFilter = filter & builder.Eq(p => p.Types.Id, catalagSpecParams.TypeId);
            }
            var totalItems = await _catalogContext.Products.CountDocumentsAsync(filter);
            var data = await _catalogContext.Products.Find(filter)
                                                     .Skip((catalagSpecParams.PageIndex-1) * catalagSpecParams.PageSize)
                                                     .Limit(catalagSpecParams.PageSize)
                                                     .ToListAsync();
            return new Pagination<Product>(
                  catalagSpecParams.PageIndex,
                  catalagSpecParams.PageSize,
                  (int)totalItems,
                  data
                  );
        }
        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            return await _catalogContext.Products.Find(p => p.Brands.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _catalogContext.Products.Find(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
            return product;
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _catalogContext.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }
        public async Task<bool> DeleteProduct(string id)
        {
           var deletedProduct = await _catalogContext.Products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _catalogContext.Brands.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _catalogContext.Types.Find(p => true).ToListAsync();
        }

    
    }
}
