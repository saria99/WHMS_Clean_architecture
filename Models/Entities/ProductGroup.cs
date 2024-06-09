using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Models.Entities
{
    public class ProductGroup : _Base
    {
        public ProductGroup() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
