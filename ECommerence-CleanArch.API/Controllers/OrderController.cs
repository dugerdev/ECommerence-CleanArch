using Microsoft.AspNetCore.Mvc;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Order;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.API.Controllers;

/// <summary>
/// Sipariş yönetimi için API endpoint'leri
/// CRUD operasyonları + sipariş durumu yönetimi
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm siparişleri sayfalı olarak getirir
    /// </summary>
    /// <param name="page">Sayfa numarası</param>
    /// <param name="size">Sayfa başına kayıt sayısı</param>
    /// <param name="customerId">Müşteri ID'sine göre filtreleme</param>
    /// <param name="status">Sipariş durumuna göre filtreleme</param>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Sayfalı sipariş listesi</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Paginate<OrderDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 0,
        [FromQuery] int size = 10,
        [FromQuery] Guid? customerId = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null)
    {
        try
        {
            Expression<Func<Order, bool>>? predicate = null;
            
            if (customerId.HasValue)
            {
                predicate = o => o.CustomerId == customerId.Value;
            }
            
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                var statusFilter = (Expression<Func<Order, bool>>)(o => o.OrderStatus == orderStatus);
                predicate = predicate == null ? statusFilter : 
                    CombineExpressions(predicate, statusFilter);
            }
            
            if (startDate.HasValue && endDate.HasValue)
            {
                var dateFilter = (Expression<Func<Order, bool>>)(o => o.OrderDate >= startDate.Value && o.OrderDate <= endDate.Value);
                predicate = predicate == null ? dateFilter : 
                    CombineExpressions(predicate, dateFilter);
            }

            var result = await _orderService.GetListAsync(
                predicate: predicate,
                orderBy: q => q.OrderByDescending(o => o.OrderDate), // En yeni siparişler önce
                index: page,
                size: size
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Siparişler getirilirken hata oluştu");
            return BadRequest("Siparişler getirilemedi");
        }
    }

    /// <summary>
    /// ID'ye göre tek sipariş getirir
    /// </summary>
    /// <param name="id">Sipariş ID'si</param>
    /// <returns>Sipariş detayı</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        try
        {
            var order = await _orderService.GetAsync(o => o.Id == id);
            
            if (order == null)
                return NotFound($"ID: {id} olan sipariş bulunamadı");

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş getirilirken hata oluştu. ID: {OrderId}", id);
            return BadRequest("Sipariş getirilemedi");
        }
    }

    /// <summary>
    /// Sipariş numarasına göre sipariş getirir
    /// </summary>
    /// <param name="orderNumber">Sipariş numarası</param>
    /// <returns>Sipariş detayı</returns>
    [HttpGet("by-number/{orderNumber}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOrderByNumber(string orderNumber)
    {
        try
        {
            var order = await _orderService.GetByOrderNumberAsync(orderNumber);
            
            if (order == null)
                return NotFound($"Sipariş numarası: {orderNumber} olan sipariş bulunamadı");

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş numarası ile sipariş getirilirken hata oluştu. OrderNumber: {OrderNumber}", orderNumber);
            return BadRequest("Sipariş getirilemedi");
        }
    }

    /// <summary>
    /// Müşteriye göre siparişleri getirir
    /// </summary>
    /// <param name="customerId">Müşteri ID'si</param>
    /// <returns>Müşteriye ait siparişler</returns>
    [HttpGet("by-customer/{customerId}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Müşteriye göre siparişler getirilirken hata oluştu. CustomerId: {CustomerId}", customerId);
            return BadRequest("Siparişler getirilemedi");
        }
    }

    /// <summary>
    /// Sipariş durumuna göre siparişleri getirir
    /// </summary>
    /// <param name="status">Sipariş durumu</param>
    /// <returns>Duruma göre siparişler</returns>
    [HttpGet("by-status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
    public async Task<IActionResult> GetOrdersByStatus(string status)
    {
        try
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                return BadRequest($"Geçersiz sipariş durumu: {status}");
            }

            var orders = await _orderService.GetOrdersByStatusAsync(orderStatus);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Duruma göre siparişler getirilirken hata oluştu. Status: {Status}", status);
            return BadRequest("Siparişler getirilemedi");
        }
    }

    /// <summary>
    /// Tarih aralığına göre siparişleri getirir
    /// </summary>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>Tarih aralığındaki siparişler</returns>
    [HttpGet("by-date-range")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
    public async Task<IActionResult> GetOrdersByDateRange(
        [FromQuery] DateTimeOffset startDate,
        [FromQuery] DateTimeOffset endDate)
    {
        try
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tarih aralığına göre siparişler getirilirken hata oluştu");
            return BadRequest("Siparişler getirilemedi");
        }
    }

    /// <summary>
    /// Yeni sipariş oluşturur
    /// </summary>
    /// <param name="createDto">Sipariş oluşturma verisi</param>
    /// <returns>Oluşturulan sipariş</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createDto)
    {
        try
        {
            // Sipariş numarası oluştur (ORD-YYYYMMDD-XXX formatında)
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            
            // Toplam tutarı hesapla (gerçek uygulamada ProductService'den fiyatlar alınmalı)
            var totalAmount = createDto.Items.Sum(item => item.Quantity * 100); // Örnek hesaplama

            var order = new Order
            {
                OrderNumber = orderNumber,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                CustomerId = createDto.CustomerId,
                ShippingAddress = createDto.ShippingAddress,
                ShippingCity = createDto.ShippingCity,
                ShippingCountry = createDto.ShippingCountry,
                ShippingPostalCode = createDto.ShippingPostalCode
            };

            var result = await _orderService.AddAsync(order);
            
            return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
            return BadRequest("Sipariş oluşturulamadı");
        }
    }

    /// <summary>
    /// Sipariş durumunu günceller
    /// </summary>
    /// <param name="id">Sipariş ID'si</param>
    /// <param name="status">Yeni durum</param>
    /// <returns>Güncellenen sipariş</returns>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
    {
        try
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                return BadRequest($"Geçersiz sipariş durumu: {status}");
            }

            var existingOrder = await _orderService.GetAsync(o => o.Id == id);
            if (existingOrder == null)
                return NotFound($"ID: {id} olan sipariş bulunamadı");

            // Sipariş durumunu güncelle
            var order = new Order
            {
                Id = id,
                OrderNumber = existingOrder.OrderNumber,
                OrderDate = existingOrder.OrderDate,
                TotalAmount = existingOrder.TotalAmount,
                OrderStatus = orderStatus,
                PaymentStatus = existingOrder.PaymentStatus,
                CustomerId = existingOrder.CustomerId,
                ShippingAddress = existingOrder.ShippingAddress,
                ShippingCity = existingOrder.ShippingCity,
                ShippingCountry = existingOrder.ShippingCountry,
                ShippingPostalCode = existingOrder.ShippingPostalCode
            };

            var result = await _orderService.UpdateAsync(order);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu. ID: {OrderId}", id);
            return BadRequest("Sipariş durumu güncellenemedi");
        }
    }

    /// <summary>
    /// Siparişi siler (soft delete)
    /// </summary>
    /// <param name="id">Sipariş ID'si</param>
    /// <returns>Silme işlemi sonucu</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        try
        {
            var existingOrder = await _orderService.GetAsync(o => o.Id == id);
            if (existingOrder == null)
                return NotFound($"ID: {id} olan sipariş bulunamadı");

            var order = new Order { Id = id };
            await _orderService.DeleteAsync(order, permanent: false);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş silinirken hata oluştu. ID: {OrderId}", id);
            return BadRequest("Sipariş silinemedi");
        }
    }

    // Helper method - Expression'ları birleştirmek için
    private static Expression<Func<Order, bool>> CombineExpressions(
        Expression<Func<Order, bool>> left, 
        Expression<Func<Order, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(Order), "o");
        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);
        var combined = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<Order, bool>>(combined, parameter);
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
