namespace CosmeticEnterpriseBack.Entities;

/// <summary>
/// Рецептура
/// </summary>
public class Recipes
{
    public long Id { get; set; }
    public List<FinishedProducts> FinishedProductsList { get; set; } = [];
}