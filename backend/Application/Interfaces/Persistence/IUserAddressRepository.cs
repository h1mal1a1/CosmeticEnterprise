using CosmeticEnterpriseBack.Domain.Entities;

namespace CosmeticEnterpriseBack.Application.Interfaces.Persistence;

/// <summary>
/// Контракт доступа к адресам пользователей.
/// Описывает, какие операции с адресами требует домен, без привязки к EF Core.
/// </summary>
public interface IUserAddressRepository
{
    /// <summary>
    /// Возвращает все адреса пользователя, отсортированные: сначала "по умолчанию", затем по дате обновления.
    /// Используется для отображения списка адресов в профиле.
    /// </summary>
    Task<IReadOnlyCollection<UserAddress>> GetByUserIdAsync(long userId, CancellationToken ct);

    /// <summary>
    /// Ищет конкретный адрес. Гарантирует, что адрес принадлежит указанному пользователю.
    /// Возвращает <c>null</c>, если не найден или принадлежит другому пользователю.
    /// </summary>
    Task<UserAddress?> GetByIdAsync(long userId, long addressId, CancellationToken ct);

    /// <summary>
    /// Проверяет, есть ли у пользователя хотя бы один сохранённый адрес.
    /// Нужен, чтобы при создании первого адреса автоматически выставить <c>IsDefault = true</c>.
    /// </summary>
    Task<bool> HasAnyAsync(long userId, CancellationToken ct);

    /// <summary>
    /// Проверяет, привязан ли адрес к существующим заказам.
    /// Блокирует удаление, если адрес используется в истории заказов (целостность данных).
    /// </summary>
    Task<bool> IsUsedInOrdersAsync(long addressId, CancellationToken ct);

    /// <summary>
    /// Возвращает самый свежий адрес пользователя (по <c>UpdatedAtUtc</c>).
    /// Используется как кандидат на новый "адрес по умолчанию", если текущий удаляется.
    /// </summary>
    Task<UserAddress?> GetMostRecentAsync(long userId, CancellationToken ct);

    /// <summary>
    /// Добавляет новую сущность адреса в контекст отслеживания (подготовка к сохранению).
    /// </summary>
    Task AddAsync(UserAddress address, CancellationToken ct);

    /// <summary>
    /// Помечает сущность адреса на удаление в контексте отслеживания.
    /// </summary>
    Task RemoveAsync(UserAddress address, CancellationToken ct);

    /// <summary>
    /// Снимает статус <c>IsDefault</c> со всех адресов пользователя.
    /// Выполняется пакетным SQL-запросом (без загрузки сущностей в память).
    /// </summary>
    Task SetDefaultFalseForUserAsync(long userId, DateTime now, CancellationToken ct);

    /// <summary>
    /// Устанавливает переданный адрес как основной. Обновляет флаг и метку времени.
    /// </summary>
    Task SetDefaultTrueAsync(UserAddress address, DateTime now, CancellationToken ct);

    /// <summary>
    /// Сохраняет все накопленные изменения в базу данных.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct);

    /// <summary>
    /// Открывает транзакцию БД. Нужна для атомарного выполнения нескольких операций (например: снять дефолт → добавить новый → сохранить).
    /// </summary>
    Task BeginTransactionAsync(CancellationToken ct);
}