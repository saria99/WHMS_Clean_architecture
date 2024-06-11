using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class CustomerCategory : _Base
    {
        public CustomerCategory() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
