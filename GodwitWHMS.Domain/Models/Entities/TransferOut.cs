using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class TransferOut : _Base
    {
        public TransferOut() { }
        public string? Number { get; set; }
        public DateTime? TransferReleaseDate { get; set; }
        public TransferStatus? Status { get; set; }
        public string? Description { get; set; }
        public int? WarehouseFromId { get; set; }
        public Warehouse? WarehouseFrom { get; set; }
        public int? WarehouseToId { get; set; }
        public Warehouse? WarehouseTo { get; set; }
    }
}
