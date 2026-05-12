using CosmeticEnterpriseBack.Domain.Entities;
using CosmeticEnterpriseBack.Domain.Interfaces;
using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Persistence.Repositories;

public class UserAddressRepository(AppDbContext db) : IUserAddressRepository
{
    /// <inheritdoc cref="IUserAddressRepository.GetByUserIdAsync"/>
    public async Task<IReadOnlyCollection<UserAddress>> GetByUserIdAsync(long userId, CancellationToken ct = default) =>
        await db.UserAddresses.AsNoTracking()
            .Where(x => x.IdUser == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.UpdatedAtUtc)
            .ThenByDescending(x => x.Id)
            .ToListAsync(ct);

    /// <inheritdoc cref="IUserAddressRepository.GetByIdAsync"/>
    public Task<UserAddress?> GetByIdAsync(long userId, long addressId, CancellationToken ct = default) =>
        db.UserAddresses.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == addressId && x.IdUser == userId, ct);

    /// <inheritdoc cref="IUserAddressRepository.HasAnyAsync"/>
    public Task<bool> HasAnyAsync(long userId, CancellationToken ct = default) =>
        db.UserAddresses.AnyAsync(x => x.IdUser == userId, ct);

    /// <inheritdoc cref="IUserAddressRepository.IsUsedInOrdersAsync"/>
    public Task<bool> IsUsedInOrdersAsync(long addressId, CancellationToken ct = default) =>
        db.Orders.AnyAsync(x => x.IdUserAddress == addressId, ct);

    /// <inheritdoc cref="IUserAddressRepository.GetMostRecentAsync"/>
    public Task<UserAddress?> GetMostRecentAsync(long userId, CancellationToken ct = default) =>
        db.UserAddresses
            .Where(x => x.IdUser == userId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ThenByDescending(x => x.Id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc cref="IUserAddressRepository.AddAsync"/>
    public Task AddAsync(UserAddress address, CancellationToken ct = default)
    {
        db.UserAddresses.Add(address);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IUserAddressRepository.RemoveAsync"/>
    public Task RemoveAsync(UserAddress address, CancellationToken ct = default)
    {
        db.UserAddresses.Remove(address);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IUserAddressRepository.SetDefaultFalseForUserAsync"/>
    public Task SetDefaultFalseForUserAsync(long userId, DateTime now, CancellationToken ct = default) =>
        db.UserAddresses
            .Where(x => x.IdUser == userId && x.IsDefault)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDefault, false).SetProperty(x => x.UpdatedAtUtc, now), ct);

    /// <inheritdoc cref="IUserAddressRepository.SetDefaultTrueAsync"/>
    public Task SetDefaultTrueAsync(UserAddress address, DateTime now, CancellationToken ct = default)
    {
        address.IsDefault = true;
        address.UpdatedAtUtc = now;
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IUserAddressRepository.SaveChangesAsync"/>
    public Task SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);

    /// <inheritdoc cref="IUserAddressRepository.BeginTransactionAsync"/>
    public Task BeginTransactionAsync(CancellationToken ct = default) =>
        db.Database.BeginTransactionAsync(ct);
}