using CosmeticEnterpriseBack.Application.Interfaces;
using CosmeticEnterpriseBack.Application.Mappers;
using CosmeticEnterpriseBack.Application.Validators;
using CosmeticEnterpriseBack.Api.DTOs.UserAddresses;
using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Interfaces;
using CosmeticEnterpriseBack.Domain.Services;

namespace CosmeticEnterpriseBack.Application.Services;

public class UserAddressAppService(
    IUserAddressRepository repository,
    IUnitOfWork unitOfWork,
    IUserAddressValidator validator,
    IUserAddressMapper mapper,
    UserAddressDomainService domainService) : IUserAddressService
{
    public async Task<IReadOnlyCollection<UserAddressResponse>> GetMyAddressesAsync(long userId, CancellationToken cancellationToken)
    {
        var addresses = await repository.GetByUserIdAsync(userId, cancellationToken);
        return mapper.ToResponseList(addresses);
    }

    public async Task<UserAddressResponse> GetMyAddressByIdAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await repository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) throw new KeyNotFoundException("User address not found.");
        return mapper.ToResponse(address);
    }

    public async Task<UserAddressResponse> CreateMyAddressAsync(long userId, CreateUserAddressRequest request, CancellationToken cancellationToken)
    {
        validator.ValidateCreate(request);

        var existingAddresses = await repository.GetByUserIdAsync(userId, cancellationToken);
        var now = DateTime.UtcNow;

        await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var address = new UserAddress
            {
                IdUser = userId,
                RecipientName = request.RecipientName.Trim(),
                Phone = request.Phone.Trim(),
                Country = request.Country.Trim(),
                City = request.City.Trim(),
                Street = request.Street.Trim(),
                House = request.House.Trim(),
                Apartment = NormalizeOptional(request.Apartment),
                PostalCode = NormalizeOptional(request.PostalCode),
                Comment = NormalizeOptional(request.Comment),
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            // Применяем чистые бизнес-правила из домена
            domainService.ApplyDefaultRules(existingAddresses, address, request.IsDefault, now);

            await repository.AddAsync(address, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return mapper.ToResponse(address);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserAddressResponse> UpdateMyAddressAsync(long userId, long addressId, UpdateUserAddressRequest request, CancellationToken cancellationToken)
    {
        validator.ValidateUpdate(request);

        var address = await repository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) throw new KeyNotFoundException("User address not found.");

        var existingAddresses = await repository.GetByUserIdAsync(userId, cancellationToken);
        var now = DateTime.UtcNow;

        await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            address.RecipientName = request.RecipientName.Trim();
            address.Phone = request.Phone.Trim();
            address.Country = request.Country.Trim();
            address.City = request.City.Trim();
            address.Street = request.Street.Trim();
            address.House = request.House.Trim();
            address.Apartment = NormalizeOptional(request.Apartment);
            address.PostalCode = NormalizeOptional(request.PostalCode);
            address.Comment = NormalizeOptional(request.Comment);
            address.UpdatedAtUtc = now;

            // Исключаем текущий адрес из коллекции, чтобы правила применялись к остальным
            var otherAddresses = existingAddresses.Where(a => a.Id != address.Id).ToList();
            domainService.ApplyDefaultRules(otherAddresses, address, request.IsDefault, now);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return mapper.ToResponse(address);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteMyAddressAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await repository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) throw new KeyNotFoundException("User address not found.");

        var isUsedInOrders = await repository.IsUsedInOrdersAsync(addressId, cancellationToken);
        if (isUsedInOrders) throw new InvalidOperationException("Address cannot be deleted because it is used in existing orders.");

        var remainingAddresses = (await repository.GetByUserIdAsync(userId, cancellationToken))
            .Where(a => a.Id != addressId)
            .ToList();

        await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await repository.RemoveAsync(address, cancellationToken);
            
            // Доменное правило: если удалили дефолтный, назначаем новый из оставшихся
            domainService.EnsureDefaultExistsAfterRemoval(remainingAddresses, DateTime.UtcNow);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserAddressResponse> SetDefaultAddressAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await repository.GetByIdAsync(userId, addressId, cancellationToken);
        if (address is null) throw new KeyNotFoundException("User address not found.");
        if (address.IsDefault) return mapper.ToResponse(address);

        var now = DateTime.UtcNow;
        await using var tx = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await repository.SetDefaultFalseForUserAsync(userId, now, cancellationToken);
            await repository.SetDefaultTrueAsync(address, now, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return mapper.ToResponse(address);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}