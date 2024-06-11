using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class GoodsReceive : _Base
    {
        public GoodsReceive() { }
        public string? Number { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public GoodsReceiveStatus? Status { get; set; }
        public string? Description { get; set; }
        public required int PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
