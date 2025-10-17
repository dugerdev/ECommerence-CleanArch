using FluentValidation;
using ECommerence_CleanArch.Application.DTOs.Product;

namespace ECommerence_CleanArch.Application.Validators.Product;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ürün ID boş olamaz");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz")
            .MaximumLength(200).WithMessage("Ürün adı maksimum 200 karakter olabilir");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz")
            .MaximumLength(1000).WithMessage("Açıklama maksimum 1000 karakter olabilir");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stok negatif olamaz");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU boş olamaz")
            .MaximumLength(50).WithMessage("SKU maksimum 50 karakter olabilir");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçilmelidir");

        RuleFor(x => x.PriceCurrency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçilmelidir");
    }
}

