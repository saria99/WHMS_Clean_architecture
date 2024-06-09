using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Models.Entities
{
    public class Tax : _Base
    {
        public Tax() { }
        public string? Name { get; set; }
        public double? Percentage { get; set; }
        public string? Description { get; set; }
    }
}
