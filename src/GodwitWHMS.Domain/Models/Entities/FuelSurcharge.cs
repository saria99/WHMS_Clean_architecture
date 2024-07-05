using GodwitWHMS.Domain.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class FuelSurcharge : _Base
    {
        public FuelSurcharge() { }

        public required int CarrierId { get; set; }
        public Carrier? Carrier { get; set; }

        public required int OriginCountryId { get; set; }
        public Country? OriginCountry { get; set; }

        public required int DestinationCountryId { get; set; }
        public Country? DestinationCountry { get; set; }

        public required DateTime EffectiveDate { get; set; }
        public required decimal FuelSurchargePercentage { get; set; }
    }
}
