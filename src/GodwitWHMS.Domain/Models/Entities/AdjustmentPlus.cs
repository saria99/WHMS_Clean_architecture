using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class AdjustmentPlus : _Base
    {
        public AdjustmentPlus() { }
        public string? Number { get; set; }
        public DateTime? AdjustmentDate { get; set; }
        public AdjustmentStatus? Status { get; set; }
        public string? Description { get; set; }
    }
}
