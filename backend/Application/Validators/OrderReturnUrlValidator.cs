namespace CosmeticEnterpriseBack.Application.Validators;

public class OrderReturnUrlValidator : IOrderReturnUrlValidator
{
    public void Validate(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return;

        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
            throw new ArgumentException("ReturnUrl must be an absolute URL.");

        if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            throw new ArgumentException("ReturnUrl must use http or https.");
    }
}