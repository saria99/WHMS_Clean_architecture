using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class ProductGroup : _Base
    {
        public ProductGroup() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
