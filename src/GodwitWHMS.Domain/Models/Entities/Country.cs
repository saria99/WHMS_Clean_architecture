using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class Country : _Base
    {
        public string CountryCode { get; set; } = default!;
        public string CountryName { get; set; } = default!;
    }
}
