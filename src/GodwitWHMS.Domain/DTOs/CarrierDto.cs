namespace GodwitWHMS.Domain.DTOs
{
    public class CarrierDto
    {
        public int Id { get; set; }
        public string CarrierName { get; set; } = default!;
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
