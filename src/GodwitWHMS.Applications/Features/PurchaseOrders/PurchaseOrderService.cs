using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.PurchaseOrders
{
    public class PurchaseOrderService : Repository<PurchaseOrder>
    {
        public PurchaseOrderService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }


        public async Task RecalculateParentAsync(int? masterId)
        {

            var master = await _context.Set<PurchaseOrder>()
                .Include(x => x.Tax)
                .Where(x => x.Id == masterId && x.IsNotDeleted == true)
                .FirstOrDefaultAsync();

            var childs = await _context.Set<PurchaseOrderItem>()
                .Where(x => x.PurchaseOrderId == masterId && x.IsNotDeleted == true)
                .ToListAsync();

            if (master != null)
            {
                master.BeforeTaxAmount = 0;
                foreach (var item in childs)
                {
                    master.BeforeTaxAmount += item.Total;
                }
                if (master.Tax != null)
                {
                    master.TaxAmount = (master.Tax.Percentage / 100.0) * master.BeforeTaxAmount;
                }
                master.AfterTaxAmount = master.BeforeTaxAmount + master.TaxAmount;
                _context.Set<PurchaseOrder>().Update(master);
                await _context.SaveChangesAsync();
            }
        }



        public override async Task UpdateAsync(PurchaseOrder? entity)
        {
            if (entity != null)
            {
                if (entity is IHasAudit auditEntity && !string.IsNullOrEmpty(_userId))
                {
                    auditEntity.UpdatedByUserId = _userId;
                }
                if (entity is IHasAudit auditedEntity)
                {
                    auditedEntity.UpdatedAtUtc = DateTime.Now;
                }

                _context.Set<PurchaseOrder>().Update(entity);
                await _context.SaveChangesAsync();

                await RecalculateParentAsync(entity.Id);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }

    }
}
