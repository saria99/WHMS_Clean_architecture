﻿using GodwitWHMS.Applications.InventoryTransactions;
using GodwitWHMS.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class InvenTransController : ODataController
    {
        private readonly InventoryTransactionService _inventoryTransactionService;

        public InvenTransController(InventoryTransactionService inventoryTransactionService)
        {
            _inventoryTransactionService = inventoryTransactionService;
        }

        [EnableQuery]
        public IQueryable<InvenTransDto> Get()
        {
            return _inventoryTransactionService
                .GetAll()
                .Include(x => x.Warehouse)
                .Include(x => x.Product)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Where(x => x.Product!.Physical == true)
                .Select(rec => new InvenTransDto
                {
                    Id = rec.Id,
                    RowGuid = rec.RowGuid,
                    CreatedAtUtc = rec.CreatedAtUtc,
                    ModuleId = rec.ModuleId,
                    ModuleName = rec.ModuleName,
                    ModuleCode = rec.ModuleCode,
                    ModuleNumber = rec.ModuleNumber,
                    MovementDate = rec.MovementDate,
                    Status = rec.Status,
                    Number = rec.Number,
                    Warehouse = rec.Warehouse!.Name,
                    Product = rec.Product!.Name,
                    Movement = rec.Movement,
                    TransType = rec.TransType,
                    Stock = rec.Stock,
                    WarehouseFrom = rec.WarehouseFrom!.Name,
                    WarehouseTo = rec.WarehouseTo!.Name,
                });
        }


    }
}
