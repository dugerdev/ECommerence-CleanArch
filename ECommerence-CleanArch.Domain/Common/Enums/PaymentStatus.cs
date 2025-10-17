namespace ECommerence_CleanArch.Domain.Common.Enums;

public enum PaymentStatus
{
    CreditCard = 0,
    DebitCard = 1,
    BankTransfer = 2,
    Cash = 3,
    Paypal = 4,
    Stripe = 5
}
