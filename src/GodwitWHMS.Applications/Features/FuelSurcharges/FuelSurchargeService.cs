using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.Features.FuelSurcharges
{
    public class FuelSurchargeService : Repository<FuelSurcharge>
    {
        public FuelSurchargeService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
            base(context, httpContextAccessor, auditColumnTransformer)
        {
        }



        public decimal GetCurrentFuelSurcharge(int carrierId, int originCountryId, int destinationCountryId)
        {
            var currentFuelSurcharge = _context.Set<FuelSurcharge>()
                .Where(fs => fs.CarrierId == carrierId &&
                             fs.OriginCountryId == originCountryId &&
                             fs.DestinationCountryId == destinationCountryId)
                .OrderByDescending(fs => fs.EffectiveDate)
                .FirstOrDefault();

            return currentFuelSurcharge != null ? currentFuelSurcharge.FuelSurchargePercentage : 0;
        }
    }
}
