using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.DTOs.Category;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Services;

/// <summary>
/// Category service interface
/// Business logic için contract
/// </summary>
public interface ICategoryService
{
    Task<Paginate<CategoryDto>> GetListAsync(
        Expression<Func<Category, bool>>? predicate = null,
        Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
        Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<CategoryDto?> GetAsync(
        Expression<Func<Category, bool>> predicate,
        Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<bool> AnyAsync(
        Expression<Func<Category, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<CategoryDto> AddAsync(Category entity);
    Task<CategoryDto> UpdateAsync(Category entity);
    Task<CategoryDto> DeleteAsync(Category entity, bool permanent = false);

    Task<IEnumerable<CategoryDto>> GetParentCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(Guid parentCategoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}
