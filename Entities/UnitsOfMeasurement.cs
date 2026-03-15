namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Единицы измерения
/// </summary>
public class UnitsOfMeasurement
{
    public long Id { get; set; }
    public List<Materials> MaterialsList { get; set; } = [];
    public List<FinishedProducts> FinishedProductsList { get; set; } = [];
}