using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.UserAddresses;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.UserAddresses;

public class UserAddressService(AppDbContext dbContext) : IUserAddressService
{
    public async Task<IReadOnlyCollection<UserAddressResponse>> GetMyAddressesAsync(long userId, CancellationToken cancellationToken)
    {
        var addresses = await dbContext.UserAddresses
            .AsNoTracking()
            .Where(x => x.IdUser == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.UpdatedAtUtc)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        return addresses
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<UserAddressResponse> GetMyAddressByIdAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await dbContext.UserAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == addressId && x.IdUser == userId,
                cancellationToken);

        if (address is null)
            throw new KeyNotFoundException("User address not found.");

        return MapToResponse(address);
    }

    public async Task<UserAddressResponse> CreateMyAddressAsync(long userId, CreateUserAddressRequest request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var hasAnyAddresses = await dbContext.UserAddresses
            .AnyAsync(x => x.IdUser == userId, cancellationToken);

        var now = DateTime.UtcNow;
        var makeDefault = request.IsDefault || !hasAnyAddresses;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (makeDefault)
            {
                await dbContext.UserAddresses
                    .Where(x => x.IdUser == userId && x.IsDefault)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(x => x.IsDefault, false)
                            .SetProperty(x => x.UpdatedAtUtc, now),
                        cancellationToken);
            }

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
                IsDefault = makeDefault,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            dbContext.UserAddresses.Add(address);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(address);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserAddressResponse> UpdateMyAddressAsync(long userId, long addressId, UpdateUserAddressRequest request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var address = await dbContext.UserAddresses
            .FirstOrDefaultAsync(
                x => x.Id == addressId && x.IdUser == userId,
                cancellationToken);

        if (address is null)
            throw new KeyNotFoundException("User address not found.");

        var now = DateTime.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (request.IsDefault && !address.IsDefault)
            {
                await dbContext.UserAddresses
                    .Where(x => x.IdUser == userId && x.IsDefault)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(x => x.IsDefault, false)
                            .SetProperty(x => x.UpdatedAtUtc, now),
                        cancellationToken);
            }

            address.RecipientName = request.RecipientName.Trim();
            address.Phone = request.Phone.Trim();
            address.Country = request.Country.Trim();
            address.City = request.City.Trim();
            address.Street = request.Street.Trim();
            address.House = request.House.Trim();
            address.Apartment = NormalizeOptional(request.Apartment);
            address.PostalCode = NormalizeOptional(request.PostalCode);
            address.Comment = NormalizeOptional(request.Comment);
            address.IsDefault = request.IsDefault;
            address.UpdatedAtUtc = now;

            await dbContext.SaveChangesAsync(cancellationToken);

            if (!request.IsDefault)
            {
                await EnsureUserHasDefaultAddressAsync(userId, cancellationToken);
                await dbContext.Entry(address).ReloadAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(address);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteMyAddressAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await dbContext.UserAddresses
            .FirstOrDefaultAsync(
                x => x.Id == addressId && x.IdUser == userId,
                cancellationToken);

        if (address is null)
            throw new KeyNotFoundException("User address not found.");

        var isUsedInOrders = await dbContext.Orders
            .AnyAsync(x => x.IdUserAddress == addressId, cancellationToken);

        if (isUsedInOrders)
        {
            throw new InvalidOperationException(
                "Address cannot be deleted because it is used in existing orders.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var wasDefault = address.IsDefault;

            dbContext.UserAddresses.Remove(address);
            await dbContext.SaveChangesAsync(cancellationToken);

            if (wasDefault)
            {
                await EnsureUserHasDefaultAddressAsync(userId, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserAddressResponse> SetDefaultAddressAsync(long userId, long addressId, CancellationToken cancellationToken)
    {
        var address = await dbContext.UserAddresses
            .FirstOrDefaultAsync(
                x => x.Id == addressId && x.IdUser == userId,
                cancellationToken);

        if (address is null)
            throw new KeyNotFoundException("User address not found.");

        if (address.IsDefault)
            return MapToResponse(address);

        var now = DateTime.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await dbContext.UserAddresses
                .Where(x => x.IdUser == userId && x.IsDefault)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(x => x.IsDefault, false)
                        .SetProperty(x => x.UpdatedAtUtc, now),
                    cancellationToken);

            address.IsDefault = true;
            address.UpdatedAtUtc = now;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return MapToResponse(address);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task EnsureUserHasDefaultAddressAsync(long userId, CancellationToken cancellationToken)
    {
        var hasDefault = await dbContext.UserAddresses
            .AnyAsync(x => x.IdUser == userId && x.IsDefault, cancellationToken);

        if (hasDefault)
            return;

        var candidate = await dbContext.UserAddresses
            .Where(x => x.IdUser == userId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ThenByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (candidate is null)
            return;

        candidate.IsDefault = true;
        candidate.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateRequest(CreateUserAddressRequest request)
    {
        ValidateRequired(request.RecipientName, nameof(request.RecipientName));
        ValidateRequired(request.Phone, nameof(request.Phone));
        ValidateRequired(request.Country, nameof(request.Country));
        ValidateRequired(request.City, nameof(request.City));
        ValidateRequired(request.Street, nameof(request.Street));
        ValidateRequired(request.House, nameof(request.House));
    }

    private static void ValidateRequest(UpdateUserAddressRequest request)
    {
        ValidateRequired(request.RecipientName, nameof(request.RecipientName));
        ValidateRequired(request.Phone, nameof(request.Phone));
        ValidateRequired(request.Country, nameof(request.Country));
        ValidateRequired(request.City, nameof(request.City));
        ValidateRequired(request.Street, nameof(request.Street));
        ValidateRequired(request.House, nameof(request.House));
    }

    private static void ValidateRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} is required.");
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static UserAddressResponse MapToResponse(UserAddress address)
    {
        return new UserAddressResponse
        {
            Id = address.Id,
            IdUser = address.IdUser,
            RecipientName = address.RecipientName,
            Phone = address.Phone,
            Country = address.Country,
            City = address.City,
            Street = address.Street,
            House = address.House,
            Apartment = address.Apartment,
            PostalCode = address.PostalCode,
            Comment = address.Comment,
            IsDefault = address.IsDefault,
            CreatedAtUtc = address.CreatedAtUtc,
            UpdatedAtUtc = address.UpdatedAtUtc
        };
    }
}