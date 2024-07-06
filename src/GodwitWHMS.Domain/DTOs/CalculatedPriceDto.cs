public class CalculatedPriceDto
{
    public int Id { get; set; }
    public int BasePriceId { get; set; }
    public string CarrierName { get; set; }
    public string OriginCountryName { get; set; }
    public string DestinationCountryName { get; set; }
    public string ServiceType { get; set; }
    public decimal PriceWithFuelSurcharge { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Weight { get; set; }
    public bool IsCheapest { get; set; }
    public Guid RowGuid { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
