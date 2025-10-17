using AutoMapper;
using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Contracts.Repositories;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Category;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Features;

/// <summary>
/// Category service implementation
/// Business logic + Repository coordination
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<CategoryDto>> GetListAsync(
        Expression<Func<Category, bool>>? predicate = null,
        Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
        Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var pagedCategories = await _categoryRepository.GetListAsync(
            predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);

        return new Paginate<CategoryDto>
        {
            Index = pagedCategories.Index,
            Size = pagedCategories.Size,
            Count = pagedCategories.Count,
            Pages = pagedCategories.Pages,
            Items = _mapper.Map<IList<CategoryDto>>(pagedCategories.Items)
        };
    }

    public async Task<CategoryDto?> GetAsync(
        Expression<Func<Category, bool>> predicate,
        Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return category != null ? _mapper.Map<CategoryDto>(category) : null;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Category, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.AnyAsync(predicate, withDeleted, enableTracking, cancellationToken);
    }

    public async Task<CategoryDto> AddAsync(Category entity)
    {
        var addedCategory = await _categoryRepository.AddAsync(entity);
        return _mapper.Map<CategoryDto>(addedCategory);
    }

    public async Task<CategoryDto> UpdateAsync(Category entity)
    {
        var updatedCategory = await _categoryRepository.UpdateAsync(entity);
        return _mapper.Map<CategoryDto>(updatedCategory);
    }

    public async Task<CategoryDto> DeleteAsync(Category entity, bool permanent = false)
    {
        var deletedCategory = await _categoryRepository.DeleteAsync(entity, permanent);
        return _mapper.Map<CategoryDto>(deletedCategory);
    }

    public async Task<IEnumerable<CategoryDto>> GetParentCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetParentCategoriesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(Guid parentCategoryId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetSubCategoriesAsync(parentCategoryId, cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetActiveCategoriesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}
