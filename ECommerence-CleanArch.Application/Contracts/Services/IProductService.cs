using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.DTOs.Product;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Services;

/// <summary>
/// Product service interface
/// Business logic için contract
/// </summary>
public interface IProductService
{
    Task<Paginate<ProductDto>> GetListAsync(
        Expression<Func<Product, bool>>? predicate = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<ProductDto?> GetAsync(
        Expression<Func<Product, bool>> predicate,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<bool> AnyAsync(
        Expression<Func<Product, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<ProductDto> AddAsync(Product entity);
    Task<ProductDto> UpdateAsync(Product entity);
    Task<ProductDto> DeleteAsync(Product entity, bool permanent = false);

    Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByCurrencyAsync(Currency currency, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default);
}


