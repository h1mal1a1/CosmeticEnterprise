namespace CosmeticEnterpriseBack.Application.Validators;

public interface IOrderReturnUrlValidator
{
    void Validate(string? returnUrl);
}