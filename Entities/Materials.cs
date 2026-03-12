namespace CosmeticEnterpriseBack.Entities;
/// <summary>
/// Материал(сырье)
/// </summary>
public class Materials
{
    public long Id { get; set; }
        
    public UnitsOfMeasurement UnitsOfMeasurement {get; set; }
    public List<SupplyPositions> SupplyPositionsList { get; set; }
}