using ECommerence_CleanArch.Domain.Common;
using ECommerence_CleanArch.Domain.Common.Enums;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive 
                && !p.IsDeleted
                ).OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId
                && p.IsActive
                && !p.IsDeleted
                )
                .ToListAsync(cancellationToken);

        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _dbSet
            .Include(p => p.Category) 
            .FirstOrDefaultAsync(p => p.SKU == sku 
                                   && !p.IsDeleted,
                                 cancellationToken);

        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Stock <=  threshold
                && p.IsActive 
                && !p.IsDeleted
                ).OrderBy(p => p.Stock)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByCurrencyAsync(Currency currency, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                 .Include(p => p.Category)
                 .Where(p => p.PriceCurrency == currency
                 && p.IsActive
                 && !p.IsDeleted
                 ).ToListAsync(cancellationToken);
        }
    }
}
