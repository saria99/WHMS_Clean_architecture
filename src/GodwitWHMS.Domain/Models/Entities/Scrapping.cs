using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class Scrapping : _Base
    {
        public Scrapping() { }
        public string? Number { get; set; }
        public DateTime? ScrappingDate { get; set; }
        public ScrappingStatus? Status { get; set; }
        public string? Description { get; set; }
        public required int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
