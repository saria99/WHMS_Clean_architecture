using GodwitWHMS.Models.Contracts;
using GodwitWHMS.Models.Enums;

namespace GodwitWHMS.Models.Entities
{
    public class AdjustmentMinus : _Base
    {
        public AdjustmentMinus() { }
        public string? Number { get; set; }
        public DateTime? AdjustmentDate { get; set; }
        public AdjustmentStatus? Status { get; set; }
        public string? Description { get; set; }
    }
}
