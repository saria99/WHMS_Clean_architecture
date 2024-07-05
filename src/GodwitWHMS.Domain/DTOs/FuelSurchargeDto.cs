namespace GodwitWHMS.Domain.DTOs
{
    public class FuelSurchargeDto
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string? CarrierName { get; set; }
        public int? OriginCountryId { get; set; }
        public string? OriginCountryName { get; set; }
        public int? DestinationCountryId { get; set; }
        public string? DestinationCountryName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal FuelSurchargePercentage { get; set; }
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
