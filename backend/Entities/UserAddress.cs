namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Адрес пользователя
/// </summary>
public class UserAddress
{
    public long Id { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public long IdUser { get; set; }
    public User User { get; set; } = null!;

    /// <summary>
    /// Получатель
    /// </summary>
    public string RecipientName { get; set; } = string.Empty;

    /// <summary>
    /// Телефон
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Страна
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Город
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Улица
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Дом
    /// </summary>
    public string House { get; set; } = string.Empty;

    /// <summary>
    /// Квартира / офис
    /// </summary>
    public string? Apartment { get; set; }

    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Адрес по умолчанию
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}