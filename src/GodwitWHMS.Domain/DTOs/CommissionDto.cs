namespace GodwitWHMS.Domain.DTOs
{
    public class CommissionDto
    {
        public int Id { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public decimal CommissionPercentage { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
