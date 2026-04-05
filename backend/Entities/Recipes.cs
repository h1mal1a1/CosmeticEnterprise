using CosmeticEnterpriseBack.Base;

namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Рецептура
/// </summary>
public class Recipes : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<FinishedProducts> FinishedProductsList { get; set; } = [];
}