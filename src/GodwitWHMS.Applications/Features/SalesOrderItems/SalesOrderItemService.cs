﻿using GodwitWHMS.Applications.SalesOrders;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.SalesOrderItems
{
    public class SalesOrderItemService : Repository<SalesOrderItem>
    {
        private readonly SalesOrderService _salesOrderService;

        public SalesOrderItemService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer,
            SalesOrderService salesOrderService) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
            _salesOrderService = salesOrderService;
        }

        public override async Task AddAsync(SalesOrderItem? entity)
        {
            if (entity != null)
            {
                if (entity is IHasAudit auditEntity && !string.IsNullOrEmpty(_userId))
                {
                    auditEntity.CreatedAtUtc = DateTime.Now;
                    auditEntity.CreatedByUserId = _userId;
                }
                entity.RecalculateTotal();
                _context.Set<SalesOrderItem>().Add(entity);
                await _context.SaveChangesAsync();

                await _salesOrderService.RecalculateParentAsync(entity.SalesOrderId);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }

        public override async Task UpdateAsync(SalesOrderItem? entity)
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
                entity.RecalculateTotal();
                _context.Set<SalesOrderItem>().Update(entity);
                await _context.SaveChangesAsync();


                await _salesOrderService.RecalculateParentAsync(entity.SalesOrderId);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }


        public override async Task DeleteByIdAsync(int? id)
        {
            if (!id.HasValue)
            {
                throw new Exception("Unable to process, id is null");
            }

            var entity = await _context.Set<SalesOrderItem>()
                .FirstOrDefaultAsync(x => x.Id == id);

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

                if (entity is IHasSoftDelete softDeleteEntity)
                {
                    softDeleteEntity.IsNotDeleted = false;
                    _context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    _context.Set<SalesOrderItem>().Remove(entity);
                }

                await _context.SaveChangesAsync();


                await _salesOrderService.RecalculateParentAsync(entity.SalesOrderId);
            }
        }

        public override async Task DeleteByRowGuidAsync(Guid? rowGuid)
        {
            if (!rowGuid.HasValue)
            {
                throw new Exception("Unable to process, row guid is null");
            }

            var entity = await _context.Set<SalesOrderItem>()
                .FirstOrDefaultAsync(x => x.RowGuid == rowGuid);

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

                if (entity is IHasSoftDelete softDeleteEntity)
                {
                    softDeleteEntity.IsNotDeleted = false;
                    _context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    _context.Set<SalesOrderItem>().Remove(entity);
                }

                await _context.SaveChangesAsync();


                await _salesOrderService.RecalculateParentAsync(entity.SalesOrderId);
            }
        }



    }
}
