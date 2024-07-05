using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GodwitWHMS.Applications.Features.BasePrices
{
    public class BasePriceService : Repository<BasePrice>
    {
        public BasePriceService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }

        public async Task CalculateAndStorePricesAsync()
        {
            var basePrices = await _context.BasePrice
                .Include(bp => bp.Carrier)
                .Include(bp => bp.OriginCountry)
                .Include(bp => bp.DestinationCountry)
                .ToListAsync();

            foreach (var bp in basePrices)
            {
                var fuelSurcharge = await _context.FuelSurcharge
                    .Where(fs => fs.CarrierId == bp.CarrierId && fs.OriginCountryId == bp.OriginCountryId && fs.DestinationCountryId == bp.DestinationCountryId)
                    .OrderByDescending(fs => fs.EffectiveDate)
                    .FirstOrDefaultAsync();

                var commission = await _context.Commission
                    .OrderByDescending(c => c.EffectiveDate)
                    .FirstOrDefaultAsync();

                if (fuelSurcharge != null && commission != null)
                {
                    var priceWithFuelSurcharge = bp.Price + (bp.Price * fuelSurcharge.FuelSurchargePercentage / 100);
                    var totalPrice = priceWithFuelSurcharge + (bp.Price * commission.CommissionPercentage / 100);

                    bp.PriceWithFuelSurcharge = priceWithFuelSurcharge;
                    bp.TotalPrice = totalPrice;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
