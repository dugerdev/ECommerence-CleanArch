using AutoMapper;
using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Contracts.Repositories;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Product;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Features;

/// <summary>
/// Product service implementation
/// Business logic + Repository coordination
/// </summary>
public class ProductService : IProductService
{
    private readonly IAsyncRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IAsyncRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<ProductDto>> GetListAsync(
        Expression<Func<Product, bool>>? predicate = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var pagedProducts = await _productRepository.GetListAsync(
            predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);

        return new Paginate<ProductDto>
        {
            Index = pagedProducts.Index,
            Size = pagedProducts.Size,
            Count = pagedProducts.Count,
            Pages = pagedProducts.Pages,
            Items = _mapper.Map<IList<ProductDto>>(pagedProducts.Items)
        };
    }

    public async Task<ProductDto?> GetAsync(
        Expression<Func<Product, bool>> predicate,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Product, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await _productRepository.AnyAsync(predicate, withDeleted, enableTracking, cancellationToken);
    }

    public async Task<ProductDto> AddAsync(Product entity)
    {
        var addedProduct = await _productRepository.AddAsync(entity);
        return _mapper.Map<ProductDto>(addedProduct);
    }

    public async Task<ProductDto> UpdateAsync(Product entity)
    {
        var updatedProduct = await _productRepository.UpdateAsync(entity);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<ProductDto> DeleteAsync(Product entity, bool permanent = false)
    {
        var deletedProduct = await _productRepository.DeleteAsync(entity, permanent);
        return _mapper.Map<ProductDto>(deletedProduct);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetListAsync(
            predicate: p => p.CategoryId == categoryId,
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products.Items);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetListAsync(
            predicate: p => p.IsActive == true,
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products.Items);
    }

    public async Task<ProductDto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(
            predicate: p => p.SKU == sku,
            cancellationToken: cancellationToken);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCurrencyAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetListAsync(
            predicate: p => p.PriceCurrency == currency,
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products.Items);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetListAsync(
            predicate: p => p.Stock <= threshold,
            cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products.Items);
    }
}
