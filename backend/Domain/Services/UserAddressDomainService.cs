using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Domain.Services;

/// <summary>
/// Инкапсулирует бизнес-правила работы с адресами пользователя.
/// Не зависит от БД, HTTP, DTO или внешних сервисов. Работает только с доменными сущностями.
/// </summary>
public class UserAddressDomainService
{
    /// <summary>
    /// Применяет правила переключения флага IsDefault.
    /// Правила:
    /// 1. Первый адрес пользователя всегда становится дефолтным.
    /// 2. Если запрошен дефолтный статус, все остальные адреса сбрасываются.
    /// </summary>
    public void ApplyDefaultRules(
        IReadOnlyCollection<UserAddress> existingAddresses,
        UserAddress targetAddress,
        bool requestedDefault,
        DateTime now)
    {
        bool shouldBeDefault = requestedDefault || existingAddresses.Count == 0;

        targetAddress.IsDefault = shouldBeDefault;
        targetAddress.UpdatedAtUtc = now;

        if (shouldBeDefault)
        {
            foreach (var address in existingAddresses)
            {
                if (address.Id != targetAddress.Id && address.IsDefault)
                {
                    address.IsDefault = false;
                    address.UpdatedAtUtc = now;
                }
            }
        }
    }

    /// <summary>
    /// Выбирает кандидата на роль нового адреса по умолчанию
    /// на основе правила "самый свежий по времени обновления".
    /// </summary>
    public UserAddress? SelectFallbackDefault(IReadOnlyCollection<UserAddress> addresses)
    {
        return addresses
            .OrderByDescending(a => a.UpdatedAtUtc)
            .ThenByDescending(a => a.Id)
            .FirstOrDefault();
    }

    /// <summary>
    /// Гарантирует инвариант: у пользователя всегда должен оставаться хотя бы один дефолтный адрес,
    /// если текущий был удалён или снят с дефолта.
    /// </summary>
    public void EnsureDefaultExistsAfterRemoval(IReadOnlyCollection<UserAddress> remainingAddresses, DateTime now)
    {
        if (remainingAddresses.Any(a => a.IsDefault)) return;

        var candidate = SelectFallbackDefault(remainingAddresses);
        if (candidate is not null)
        {
            candidate.IsDefault = true;
            candidate.UpdatedAtUtc = now;
        }
    }
}