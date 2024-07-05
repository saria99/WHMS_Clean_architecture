public class CalculatedPriceDto
{
    public int Id { get; set; }
    public int BasePriceId { get; set; }
    public ServiceType ServiceType { get; set; }
    public decimal PriceWithFuelSurcharge { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsCheapest { get; set; }
    public Guid? RowGuid { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
}
