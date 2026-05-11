using Microsoft.EntityFrameworkCore.Storage;

namespace CosmeticEnterpriseBack.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}