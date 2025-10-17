using FluentValidation;
using ECommerence_CleanArch.Application.DTOs.Customer;

namespace ECommerence_CleanArch.Application.Validators.Customer;

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Müşteri ID boş olamaz");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(100).WithMessage("Ad maksimum 100 karakter olabilir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz")
            .MaximumLength(100).WithMessage("Soyad maksimum 100 karakter olabilir");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz")
            .MaximumLength(20).WithMessage("Telefon numarası maksimum 20 karakter olabilir")
            .Matches(@"^[\d\s\-\+\(\)]+$").WithMessage("Geçerli bir telefon numarası giriniz");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres boş olamaz")
            .MaximumLength(500).WithMessage("Adres maksimum 500 karakter olabilir");
    }
}

