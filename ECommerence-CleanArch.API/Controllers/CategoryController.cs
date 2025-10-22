using Microsoft.AspNetCore.Mvc;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Category;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.API.Controllers;

/// <summary>
/// Kategori yönetimi için API endpoint'leri
/// Hiyerarşik kategori yapısı + CRUD operasyonları
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm kategorileri sayfalı olarak getirir
    /// </summary>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="size">Sayfa başına kayıt sayısı</param>
    /// <param name="search">Kategori adında arama</param>
    /// <param name="isActive">Sadece aktif kategoriler</param>
    /// <returns>Sayfalı kategori listesi</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Paginate<CategoryDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCategories(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null)
    {
        try
        {
            Expression<Func<Category, bool>>? predicate = null;
            
            if (!string.IsNullOrEmpty(search))
            {
                predicate = c => c.Name.Contains(search);
            }
            
            if (isActive.HasValue)
            {
                var activeFilter = (Expression<Func<Category, bool>>)(c => c.IsActive == isActive.Value);
                predicate = predicate == null ? activeFilter : 
                    CombineExpressions(predicate, activeFilter);
            }

            var result = await _categoryService.GetListAsync(
                predicate: predicate,
                orderBy: q => q.OrderBy(c => c.Name),
                index: page,
                size: size
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategoriler getirilirken hata oluştu");
            return BadRequest("Kategoriler getirilemedi");
        }
    }

    /// <summary>
    /// ID'ye göre tek kategori getirir
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <returns>Kategori detayı</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        try
        {
            var category = await _categoryService.GetAsync(c => c.Id == id);
            
            if (category == null)
                return NotFound($"ID: {id} olan kategori bulunamadı");

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori getirilirken hata oluştu. ID: {CategoryId}", id);
            return BadRequest("Kategori getirilemedi");
        }
    }

    /// <summary>
    /// Ana kategorileri getirir (ParentCategoryId = null olanlar)
    /// </summary>
    /// <returns>Ana kategori listesi</returns>
    [HttpGet("parent")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<IActionResult> GetParentCategories()
    {
        try
        {
            var categories = await _categoryService.GetParentCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ana kategoriler getirilirken hata oluştu");
            return BadRequest("Ana kategoriler getirilemedi");
        }
    }

    /// <summary>
    /// Alt kategorileri getirir
    /// </summary>
    /// <param name="parentId">Ana kategori ID'si</param>
    /// <returns>Alt kategori listesi</returns>
    [HttpGet("sub/{parentId}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<IActionResult> GetSubCategories(Guid parentId)
    {
        try
        {
            var categories = await _categoryService.GetSubCategoriesAsync(parentId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Alt kategoriler getirilirken hata oluştu. ParentId: {ParentId}", parentId);
            return BadRequest("Alt kategoriler getirilemedi");
        }
    }

    /// <summary>
    /// Aktif kategorileri getirir
    /// </summary>
    /// <returns>Aktif kategori listesi</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<IActionResult> GetActiveCategories()
    {
        try
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif kategoriler getirilirken hata oluştu");
            return BadRequest("Aktif kategoriler getirilemedi");
        }
    }

    /// <summary>
    /// Yeni kategori oluşturur
    /// </summary>
    /// <param name="createDto">Kategori oluşturma verisi</param>
    /// <returns>Oluşturulan kategori</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createDto)
    {
        try
        {
            // DTO'yu Entity'ye dönüştür
            var category = new Category
            {
                Name = createDto.Name,
                Description = createDto.Description,
                ParentCategoryId = createDto.ParentCategoryId == Guid.Empty ? null : createDto.ParentCategoryId,
                IsActive = true // Yeni kategoriler aktif olarak oluşturulur
            };

            var result = await _categoryService.AddAsync(category);
            
            return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori oluşturulurken hata oluştu");
            return BadRequest("Kategori oluşturulamadı");
        }
    }

    /// <summary>
    /// Kategori günceller
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <param name="updateDto">Güncelleme verisi</param>
    /// <returns>Güncellenen kategori</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto updateDto)
    {
        try
        {
            if (id != updateDto.Id)
                return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor");

            var existingCategory = await _categoryService.GetAsync(c => c.Id == id);
            if (existingCategory == null)
                return NotFound($"ID: {id} olan kategori bulunamadı");

            var category = new Category
            {
                Id = updateDto.Id,
                Name = updateDto.Name,
                Description = updateDto.Description,
                ParentCategoryId = updateDto.ParentCategoryId == Guid.Empty ? null : updateDto.ParentCategoryId,
                IsActive = updateDto.IsActive
            };

            var result = await _categoryService.UpdateAsync(category);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori güncellenirken hata oluştu. ID: {CategoryId}", id);
            return BadRequest("Kategori güncellenemedi");
        }
    }

    /// <summary>
    /// Kategoriyi siler (soft delete)
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            var existingCategory = await _categoryService.GetAsync(c => c.Id == id);
            if (existingCategory == null)
                return NotFound($"ID: {id} olan kategori bulunamadı");

            var category = new Category { Id = id };
            await _categoryService.DeleteAsync(category, permanent: false);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori silinirken hata oluştu. ID: {CategoryId}", id);
            return BadRequest("Kategori silinemedi");
        }
    }

    // Helper method - Expression'ları birleştirmek için
    private static Expression<Func<Category, bool>> CombineExpressions(
        Expression<Func<Category, bool>> left, 
        Expression<Func<Category, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Category), "c");
        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);
        var combined = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<Category, bool>>(combined, parameter);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParam, ParameterExpression newParam)
    {
        return new ParameterReplacer(oldParam, newParam).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParam;
        private readonly ParameterExpression _newParam;

        public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
        {
            _oldParam = oldParam;
            _newParam = newParam;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParam ? _newParam : base.VisitParameter(node);
        }
    }
}
