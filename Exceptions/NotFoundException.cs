namespace CosmeticEnterpriseBack.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message)
    {
        
    }
}