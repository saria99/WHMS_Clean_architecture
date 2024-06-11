using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class CustomerGroup : _Base
    {
        public CustomerGroup() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
