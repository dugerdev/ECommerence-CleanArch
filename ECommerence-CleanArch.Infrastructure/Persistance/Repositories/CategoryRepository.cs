using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;


public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    
    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.Name) 
            .ToListAsync(cancellationToken);
    }

    
    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(
        Guid parentCategoryId, // ← Guid (Category entity'de ParentCategoryId Guid?)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == parentCategoryId 
                     && c.IsActive
                     && !c.IsDeleted)
            .ToListAsync(cancellationToken);

       
    }

    
    public async Task<Category?> GetCategoryWithProductsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Products) 
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        
    }

    
    public async Task<IEnumerable<Category>> GetParentCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == null
                     && c.IsActive
                     && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

       
    }
}