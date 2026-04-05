namespace CosmeticEnterpriseBack.Interfaces;

public interface ICrudService<TResponse, in TCreateRequest, in TUpdateRequest, in TKey>
{
    Task<IReadOnlyCollection<TResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TResponse?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TResponse> CreateAsync(TCreateRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> UpdateAsync(TKey id, TUpdateRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}