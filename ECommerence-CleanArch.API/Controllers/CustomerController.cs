using Microsoft.AspNetCore.Mvc;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Customer;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.API.Controllers;

/// <summary>
/// Müşteri yönetimi için API endpoint'leri
/// CRUD operasyonları + arama ve filtreleme
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm müşterileri sayfalı olarak getirir
    /// </summary>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="size">Sayfa başına kayıt sayısı</param>
    /// <param name="search">Ad, soyad veya email'de arama</param>
    /// <param name="city">Şehre göre filtreleme</param>
    /// <returns>Sayfalı müşteri listesi</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Paginate<CustomerDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? city = null)
    {
        try
        {
            Expression<Func<Customer, bool>>? predicate = null;
            
            if (!string.IsNullOrEmpty(search))
            {
                predicate = c => c.FirstName.Contains(search) || 
                               c.LastName.Contains(search) || 
                               c.Email.Contains(search);
            }
            
            if (!string.IsNullOrEmpty(city))
            {
                var cityFilter = (Expression<Func<Customer, bool>>)(c => c.City.Contains(city));
                predicate = predicate == null ? cityFilter : 
                    CombineExpressions(predicate, cityFilter);
            }

            var result = await _customerService.GetListAsync(
                predicate: predicate,
                orderBy: q => q.OrderBy(c => c.FirstName).ThenBy(c => c.LastName),
                index: page,
                size: size
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteriler getirilirken hata oluştu");
            return BadRequest("Müşteriler getirilemedi");
        }
    }

    /// <summary>
    /// ID'ye göre tek müşteri getirir
    /// </summary>
    /// <param name="id">Müşteri ID'si</param>
    /// <returns>Müşteri detayı</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        try
        {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            
            if (customer == null)
                return NotFound($"ID: {id} olan müşteri bulunamadı");

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteri getirilirken hata oluştu. ID: {CustomerId}", id);
            return BadRequest("Müşteri getirilemedi");
        }
    }

    /// <summary>
    /// Email'e göre müşteri getirir
    /// </summary>
    /// <param name="email">Müşteri email adresi</param>
    /// <returns>Müşteri detayı</returns>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(CustomerDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomerByEmail(string email)
    {
        try
        {
            var customer = await _customerService.GetByEmailAsync(email);
            
            if (customer == null)
                return NotFound($"Email: {email} olan müşteri bulunamadı");

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email ile müşteri getirilirken hata oluştu. Email: {Email}", email);
            return BadRequest("Müşteri getirilemedi");
        }
    }

    /// <summary>
    /// Aktif müşterileri getirir
    /// </summary>
    /// <returns>Aktif müşteri listesi</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), 200)]
    public async Task<IActionResult> GetActiveCustomers()
    {
        try
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif müşteriler getirilirken hata oluştu");
            return BadRequest("Aktif müşteriler getirilemedi");
        }
    }

    /// <summary>
    /// Şehre göre müşterileri getirir
    /// </summary>
    /// <param name="city">Şehir adı</param>
    /// <returns>Şehre göre müşteri listesi</returns>
    [HttpGet("by-city/{city}")]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), 200)]
    public async Task<IActionResult> GetCustomersByCity(string city)
    {
        try
        {
            var customers = await _customerService.GetCustomersByCityAsync(city);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şehre göre müşteriler getirilirken hata oluştu. City: {City}", city);
            return BadRequest("Müşteriler getirilemedi");
        }
    }

    /// <summary>
    /// Yeni müşteri oluşturur
    /// </summary>
    /// <param name="createDto">Müşteri oluşturma verisi</param>
    /// <returns>Oluşturulan müşteri</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createDto)
    {
        try
        {
            // Email benzersizlik kontrolü
            var existingCustomer = await _customerService.GetByEmailAsync(createDto.Email);
            if (existingCustomer != null)
                return BadRequest($"Email: {createDto.Email} zaten kayıtlı");

            var customer = new Customer
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                Address = createDto.Address,
                City = createDto.City,
                Country = createDto.Country,
                PostalCode = createDto.PostalCode
            };

            var result = await _customerService.AddAsync(customer);
            
            return CreatedAtAction(nameof(GetCustomer), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteri oluşturulurken hata oluştu");
            return BadRequest("Müşteri oluşturulamadı");
        }
    }

    /// <summary>
    /// Müşteri günceller
    /// </summary>
    /// <param name="id">Müşteri ID'si</param>
    /// <param name="updateDto">Güncelleme verisi</param>
    /// <returns>Güncellenen müşteri</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CustomerDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto updateDto)
    {
        try
        {
            if (id != updateDto.Id)
                return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor");

            var existingCustomer = await _customerService.GetAsync(c => c.Id == id);
            if (existingCustomer == null)
                return NotFound($"ID: {id} olan müşteri bulunamadı");

            var customer = new Customer
            {
                Id = updateDto.Id,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                Email = existingCustomer.Email, // Email değiştirilemez
                PhoneNumber = updateDto.PhoneNumber,
                Address = updateDto.Address,
                City = updateDto.City,
                Country = updateDto.Country,
                PostalCode = updateDto.PostalCode
            };

            var result = await _customerService.UpdateAsync(customer);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteri güncellenirken hata oluştu. ID: {CustomerId}", id);
            return BadRequest("Müşteri güncellenemedi");
        }
    }

    /// <summary>
    /// Müşteriyi siler (soft delete)
    /// </summary>
    /// <param name="id">Müşteri ID'si</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        try
        {
            var existingCustomer = await _customerService.GetAsync(c => c.Id == id);
            if (existingCustomer == null)
                return NotFound($"ID: {id} olan müşteri bulunamadı");

            var customer = new Customer { Id = id };
            await _customerService.DeleteAsync(customer, permanent: false);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteri silinirken hata oluştu. ID: {CustomerId}", id);
            return BadRequest("Müşteri silinemedi");
        }
    }

    // Helper method - Expression'ları birleştirmek için
    private static Expression<Func<Customer, bool>> CombineExpressions(
        Expression<Func<Customer, bool>> left, 
        Expression<Func<Customer, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Customer), "c");
        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);
        var combined = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<Customer, bool>>(combined, parameter);
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
