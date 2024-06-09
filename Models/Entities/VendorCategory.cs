using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Models.Entities
{
    public class VendorCategory : _Base
    {
        public VendorCategory() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
