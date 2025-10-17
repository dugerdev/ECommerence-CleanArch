using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{

    Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    // DÜZELTME: int → Guid? (Category entity'de ParentCategoryId artık Guid?)
    Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentCategoryId, CancellationToken cancellationToken = default); 

    Task<Category?> GetCategoryWithProductsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken = default);
}
