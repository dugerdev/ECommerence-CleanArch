using AutoMapper;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Application.DTOs.Product;
using ECommerence_CleanArch.Application.DTOs.Category;
using ECommerence_CleanArch.Application.DTOs.Customer;
using ECommerence_CleanArch.Application.DTOs.Order;

namespace ECommerence_CleanArch.Application.Mapping;

/// <summary>
/// AutoMapper mapping profili
/// Entity ↔ DTO dönüşüm kurallarını tanımlar
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // PRODUCT MAPPINGS
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Product → ProductDto (Okuma için)
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName,              // DTO'daki CategoryName
                       opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))  // Entity'deki Category.Name'den al
            .ForMember(dest => dest.PriceCurrency,
                       opt => opt.MapFrom(src => src.PriceCurrency.ToString())); // Enum → String
        
        // CreateProductDto → Product (Oluşturma için)
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())   // Id otomatik oluşur
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // CreatedAt otomatik
            .ForMember(dest => dest.Category, opt => opt.Ignore()); // Navigation property ignore
        
        // UpdateProductDto → Product (Güncelleme için)
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // CreatedAt değişmez
            .ForMember(dest => dest.Category, opt => opt.Ignore());
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // CATEGORY MAPPINGS
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Category → CategoryDto
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ParentCategoryName,
                       opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : ""))
            .ForMember(dest => dest.SubCategoryCount,
                       opt => opt.MapFrom(src => src.SubCategories.Count));
        
        // CreateCategoryDto → Category
        CreateMap<CreateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore());
        
        // UpdateCategoryDto → Category
        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore());
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // CUSTOMER MAPPINGS
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Customer → CustomerDto
        CreateMap<Customer, CustomerDto>();
        
        // CreateCustomerDto → Customer
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.ShoppingCarts, opt => opt.Ignore());
        
        // UpdateCustomerDto → Customer
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Email, opt => opt.Ignore()) // Email değiştirilemez
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.ShoppingCarts, opt => opt.Ignore());
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ORDER MAPPINGS
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Order → OrderDto
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderStatus,
                       opt => opt.MapFrom(src => src.OrderStatus.ToString())) // Enum → String
            .ForMember(dest => dest.PaymentStatus,
                       opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.CustomerName,
                       opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : ""))
            .ForMember(dest => dest.CustomerEmail,
                       opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : ""));
        
        // OrderItem → OrderItemDto
        CreateMap<OrderItem, OrderItemDto>();
        
        // CreateOrderDto → Order (Service'te manuel yapılacak, çok karmaşık)
        // CreateOrderItemDto → OrderItem (Service'te manuel yapılacak)
    }
}