using GodwitWHMS.Models.Enums;

namespace GodwitWHMS.DTOs
{
    public class GoodsReceiveDto
    {
        public int? Id { get; set; }
        public string? Number { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public GoodsReceiveStatus? Status { get; set; }
        public string? PurchaseOrder { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Vendor { get; set; }
        public Guid? RowGuid { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
