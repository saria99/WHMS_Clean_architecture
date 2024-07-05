using GodwitWHMS.Domain.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class Commission: _Base
    {

        [Required]
        public string ServiceType { get; set; } = string.Empty; // e.g., Economy, Express, etc.

        [Required]
        public decimal CommissionPercentage { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }
    }
}
