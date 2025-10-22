using Microsoft.AspNetCore.Mvc;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Product;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.API.Controllers;

/// <summary>
/// Ürün yönetimi için API endpoint'leri
/// CRUD operasyonları + özel iş mantığı
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm ürünleri sayfalı olarak getirir
    /// </summary>
    /// <param name="page">Sayfa numarası (0'dan başlar)</param>
    /// <param name="size">Sayfa başına kayıt sayısı</param>
    /// <param name="search">Ürün adında arama</param>
    /// <param name="categoryId">Kategori ID'sine göre filtreleme</param>
    /// <param name="isActive">Sadece aktif ürünler</param>
    /// <returns>Sayfalı ürün listesi</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Paginate<ProductDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10,
        [FromQuery] string? search = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] bool? isActive = null)
    {
        try
        {
            // Filtreleme koşulları
            Expression<Func<Product, bool>>? predicate = null;
            
            if (!string.IsNullOrEmpty(search))
            {
                predicate = p => p.Name.Contains(search);
            }
            
            if (categoryId.HasValue)
            {
                var categoryFilter = (Expression<Func<Product, bool>>)(p => p.CategoryId == categoryId.Value);
                predicate = predicate == null ? categoryFilter : 
                    CombineExpressions(predicate, categoryFilter);
            }
            
            if (isActive.HasValue)
            {
                var activeFilter = (Expression<Func<Product, bool>>)(p => p.IsActive == isActive.Value);
                predicate = predicate == null ? activeFilter : 
                    CombineExpressions(predicate, activeFilter);
            }

            var result = await _productService.GetListAsync(
                predicate: predicate,
                orderBy: q => q.OrderBy(p => p.Name), // İsme göre sırala
                index: page,
                size: size
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler getirilirken hata oluştu");
            return BadRequest("Ürünler getirilemedi");
        }
    }

    /// <summary>
    /// ID'ye göre tek ürün getirir
    /// </summary>
    /// <param name="id">Ürün ID'si</param>
    /// <returns>Ürün detayı</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        try
        {
            var product = await _productService.GetAsync(p => p.Id == id);
            
            if (product == null)
                return NotFound($"ID: {id} olan ürün bulunamadı");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün getirilirken hata oluştu. ID: {ProductId}", id);
            return BadRequest("Ürün getirilemedi");
        }
    }

    /// <summary>
    /// SKU'ya göre ürün getirir
    /// </summary>
    /// <param name="sku">Ürün SKU kodu</param>
    /// <returns>Ürün detayı</returns>
    [HttpGet("by-sku/{sku}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProductBySku(string sku)
    {
        try
        {
            var product = await _productService.GetBySkuAsync(sku);
            
            if (product == null)
                return NotFound($"SKU: {sku} olan ürün bulunamadı");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SKU ile ürün getirilirken hata oluştu. SKU: {Sku}", sku);
            return BadRequest("Ürün getirilemedi");
        }
    }

    /// <summary>
    /// Kategoriye göre ürünleri getirir
    /// </summary>
    /// <param name="categoryId">Kategori ID'si</param>
    /// <returns>Kategoriye ait ürünler</returns>
    [HttpGet("by-category/{categoryId}")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetProductsByCategory(Guid categoryId)
    {
        try
        {
            var products = await _productService.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategoriye göre ürünler getirilirken hata oluştu. CategoryId: {CategoryId}", categoryId);
            return BadRequest("Ürünler getirilemedi");
        }
    }

    /// <summary>
    /// Aktif ürünleri getirir
    /// </summary>
    /// <returns>Aktif ürün listesi</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetActiveProducts()
    {
        try
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif ürünler getirilirken hata oluştu");
            return BadRequest("Aktif ürünler getirilemedi");
        }
    }

    /// <summary>
    /// Para birimine göre ürünleri getirir
    /// </summary>
    /// <param name="currency">Para birimi (USD, EUR, TRY, GBP)</param>
    /// <returns>Para birimine göre ürünler</returns>
    [HttpGet("by-currency/{currency}")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetProductsByCurrency(string currency)
    {
        try
        {
            if (!Enum.TryParse<Currency>(currency, true, out var currencyEnum))
            {
                return BadRequest($"Geçersiz para birimi: {currency}");
            }

            var products = await _productService.GetProductsByCurrencyAsync(currencyEnum);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Para birimine göre ürünler getirilirken hata oluştu. Currency: {Currency}", currency);
            return BadRequest("Ürünler getirilemedi");
        }
    }

    /// <summary>
    /// Düşük stoklu ürünleri getirir
    /// </summary>
    /// <param name="threshold">Stok eşik değeri</param>
    /// <returns>Düşük stoklu ürünler</returns>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 10)
    {
        try
        {
            var products = await _productService.GetLowStockProductsAsync(threshold);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Düşük stoklu ürünler getirilirken hata oluştu. Threshold: {Threshold}", threshold);
            return BadRequest("Ürünler getirilemedi");
        }
    }

    /// <summary>
    /// Yeni ürün oluşturur
    /// </summary>
    /// <param name="createDto">Ürün oluşturma verisi</param>
    /// <returns>Oluşturulan ürün</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
    {
        try
        {
            // DTO'yu Entity'ye dönüştür
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                PriceCurrency = Enum.Parse<Currency>(createDto.PriceCurrency),
                Stock = createDto.Stock,
                SKU = createDto.SKU,
                ImageUrl = createDto.ImageUrl,
                CategoryId = createDto.CategoryId,
                IsActive = true // Yeni ürünler aktif olarak oluşturulur
            };

            var result = await _productService.AddAsync(product);
            
            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün oluşturulurken hata oluştu");
            return BadRequest("Ürün oluşturulamadı");
        }
    }

    /// <summary>
    /// Ürün günceller
    /// </summary>
    /// <param name="id">Ürün ID'si</param>
    /// <param name="updateDto">Güncelleme verisi</param>
    /// <returns>Güncellenen ürün</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateDto)
    {
        try
        {
            // ID'lerin eşleştiğini kontrol et
            if (id != updateDto.Id)
                return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor");

            // Önce ürünün var olup olmadığını kontrol et
            var existingProduct = await _productService.GetAsync(p => p.Id == id);
            if (existingProduct == null)
                return NotFound($"ID: {id} olan ürün bulunamadı");

            // DTO'yu Entity'ye dönüştür
            var product = new Product
            {
                Id = updateDto.Id,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                PriceCurrency = Enum.Parse<Currency>(updateDto.PriceCurrency),
                Stock = updateDto.Stock,
                SKU = updateDto.SKU,
                ImageUrl = updateDto.ImageUrl,
                CategoryId = updateDto.CategoryId,
                IsActive = updateDto.IsActive
            };

            var result = await _productService.UpdateAsync(product);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün güncellenirken hata oluştu. ID: {ProductId}", id);
            return BadRequest("Ürün güncellenemedi");
        }
    }

    /// <summary>
    /// Ürünü siler (soft delete)
    /// </summary>
    /// <param name="id">Ürün ID'si</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            // Önce ürünün var olup olmadığını kontrol et
            var existingProduct = await _productService.GetAsync(p => p.Id == id);
            if (existingProduct == null)
                return NotFound($"ID: {id} olan ürün bulunamadı");

            // Entity oluştur (sadece ID gerekli)
            var product = new Product { Id = id };
            
            await _productService.DeleteAsync(product, permanent: false); // Soft delete
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün silinirken hata oluştu. ID: {ProductId}", id);
            return BadRequest("Ürün silinemedi");
        }
    }

    /// <summary>
    /// Ürünü kalıcı olarak siler
    /// </summary>
    /// <param name="id">Ürün ID'si</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}/permanent")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProductPermanently(Guid id)
    {
        try
        {
            // Önce ürünün var olup olmadığını kontrol et
            var existingProduct = await _productService.GetAsync(p => p.Id == id);
            if (existingProduct == null)
                return NotFound($"ID: {id} olan ürün bulunamadı");

            // Entity oluştur (sadece ID gerekli)
            var product = new Product { Id = id };
            
            await _productService.DeleteAsync(product, permanent: true); // Hard delete
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün kalıcı olarak silinirken hata oluştu. ID: {ProductId}", id);
            return BadRequest("Ürün silinemedi");
        }
    }

    // Helper method - Expression'ları birleştirmek için
    private static Expression<Func<Product, bool>> CombineExpressions(
        Expression<Func<Product, bool>> left, 
        Expression<Func<Product, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Product), "p");
        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);
        var combined = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<Product, bool>>(combined, parameter);
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
