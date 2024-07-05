namespace GodwitWHMS.Domain.DTOs
{
    public class BasePriceDto
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string? CarrierName { get; set; }
        public int? OriginCountryId { get; set; }
        public string? OriginCountryName { get; set; }
        public int? DestinationCountryId { get; set; }
        public string? DestinationCountryName { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal PriceWithFuelSurcharge { get; set; } 
        public decimal TotalPrice { get; set; }
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
