using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

/// <summary>
/// Category-specific repository interface
/// Generic base + özel metodlar
/// </summary>
public interface ICategoryRepository : IAsyncRepository<Category>
{
    /// <summary>
    /// Ana kategorileri getir (ParentCategoryId null olanlar)
    /// </summary>
    Task<IEnumerable<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Alt kategorileri getir
    /// </summary>
    Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentCategoryId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif kategorileri getir
    /// </summary>
    Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kategori adına göre getir
    /// </summary>
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif kategorileri sayfalanmış olarak getir
    /// </summary>
    Task<Paginate<Category>> GetPagedActiveCategoriesAsync(
        int index = 0, 
        int size = 10, 
        CancellationToken cancellationToken = default);
}