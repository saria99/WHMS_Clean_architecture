using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class VendorCategory : _Base
    {
        public VendorCategory() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
