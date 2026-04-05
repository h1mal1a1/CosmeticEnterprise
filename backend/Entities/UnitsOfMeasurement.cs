using CosmeticEnterpriseBack.Base;

namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Единицы измерения
/// </summary>
public class UnitsOfMeasurement : IEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Materials> MaterialsList { get; set; } = [];
    public List<FinishedProducts> FinishedProductsList { get; set; } = [];
}