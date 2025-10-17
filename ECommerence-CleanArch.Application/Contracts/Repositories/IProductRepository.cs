using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

/// <summary>
/// Product-specific repository interface
/// Generic base + özel metodlar
/// </summary>
public interface IProductRepository : IAsyncRepository<Product>
{
    /// <summary>
    /// Kategori ID'sine göre ürünleri getir
    /// </summary>
    Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif ürünleri getir
    /// </summary>
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// SKU'ya göre ürün getir
    /// </summary>
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Para birimine göre ürünleri getir
    /// </summary>
    Task<IEnumerable<Product>> GetProductsByCurrencyAsync(Currency currency, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Düşük stoklu ürünleri getir
    /// </summary>
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kategoriye göre sayfalanmış ürünleri getir
    /// </summary>
    Task<Paginate<Product>> GetPagedProductsByCategoryAsync(
        Guid categoryId, 
        int index = 0, 
        int size = 10, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif ürünleri sayfalanmış olarak getir
    /// </summary>
    Task<Paginate<Product>> GetPagedActiveProductsAsync(
        int index = 0, 
        int size = 10, 
        CancellationToken cancellationToken = default);
}