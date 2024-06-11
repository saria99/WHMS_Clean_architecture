using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class DeliveryOrder : _Base
    {
        public DeliveryOrder() { }
        public string? Number { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DeliveryOrderStatus? Status { get; set; }
        public string? Description { get; set; }
        public required int SalesOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }
    }
}
