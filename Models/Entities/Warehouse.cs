using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Models.Entities
{
    public class Warehouse : _Base
    {
        public Warehouse() { }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool SystemWarehouse { get; set; } = false;
    }
}
