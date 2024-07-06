using GodwitWHMS.Applications.Features.Commissions;
using GodwitWHMS.Applications.Features.FuelSurcharges;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GodwitWHMS.Applications.Features.BasePrices
{
    public class CalculatedPriceService : Repository<CalculatedPrice>
    {
        private readonly FuelSurchargeService _fuelSurchargeService;
        private readonly CommissionService _commissionService;
        private readonly BasePriceService _basePriceService;

        public CalculatedPriceService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer,
            FuelSurchargeService fuelSurchargeService,
            CommissionService commissionService,
            BasePriceService basePriceService) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
            _fuelSurchargeService = fuelSurchargeService;
            _commissionService = commissionService;
            _basePriceService = basePriceService;
        }
        public async Task CalculateAndStorePricesAsync()
        {
            var basePrices = await _basePriceService.GetAll().ToListAsync();

            foreach (var basePrice in basePrices)
            {
                foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
                {
                    var fuelSurchargePercentage = _fuelSurchargeService.GetCurrentFuelSurcharge(basePrice.CarrierId, basePrice.OriginCountryId, basePrice.DestinationCountryId);
                    var commissionPercentage = _commissionService.GetCurrentCommission(serviceType.ToString());

                    var priceWithFuelSurcharge = basePrice.Price + (basePrice.Price * fuelSurchargePercentage / 100);
                    var totalPrice = priceWithFuelSurcharge + (priceWithFuelSurcharge * commissionPercentage / 100);

                    var existingCalculatedPrice = await _context.CalculatedPrice
                        .FirstOrDefaultAsync(cp => cp.BasePriceId == basePrice.Id && cp.ServiceType == serviceType && cp.Weight == basePrice.Weight);

                    if (existingCalculatedPrice != null)
                    {
                        existingCalculatedPrice.PriceWithFuelSurcharge = priceWithFuelSurcharge;
                        existingCalculatedPrice.TotalPrice = totalPrice;
                        existingCalculatedPrice.UpdatedAtUtc = DateTime.UtcNow;
                        _context.CalculatedPrice.Update(existingCalculatedPrice);
                    }
                    else
                    {
                        var calculatedPrice = new CalculatedPrice
                        {
                            BasePriceId = basePrice.Id,
                            ServiceType = serviceType,
                            Weight = basePrice.Weight,
                            PriceWithFuelSurcharge = priceWithFuelSurcharge,
                            TotalPrice = totalPrice,
                            IsCheapest = false,
                            RowGuid = Guid.NewGuid(),
                            CreatedAtUtc = DateTime.UtcNow
                        };

                        await AddAsync(calculatedPrice);
                    }
                }
            }

            // Flag the cheapest price for each weight and service type
            var serviceTypes = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().ToList();
            var weights = basePrices.Select(bp => bp.Weight).Distinct().ToList();

            foreach (var serviceType in serviceTypes)
            {
                foreach (var weight in weights)
                {
                    var cheapestPrices = await _context.CalculatedPrice
                        .Where(cp => cp.ServiceType == serviceType && cp.Weight == weight)
                        .OrderBy(cp => cp.TotalPrice)
                        .ToListAsync();

                    if (cheapestPrices.Any())
                    {
                        var cheapestPrice = cheapestPrices.First();
                        cheapestPrice.IsCheapest = true;
                        _context.CalculatedPrice.Update(cheapestPrice);

                        foreach (var price in cheapestPrices.Skip(1))
                        {
                            price.IsCheapest = false;
                            _context.CalculatedPrice.Update(price);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }


        public IQueryable<CalculatedPrice> GetAllCheapestPrices()
        {
            return _context.CalculatedPrice
                .Where(cp => cp.IsCheapest)
                .AsQueryable();
        }

    }
}
