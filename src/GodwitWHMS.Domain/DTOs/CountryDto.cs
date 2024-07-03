namespace GodwitWHMS.Domain.DTOs
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string CountryCode { get; set; } = default!;
        public string CountryName { get; set; } = default!;
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
