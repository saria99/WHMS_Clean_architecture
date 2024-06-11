﻿using AutoMapper;
using GodwitWHMS.Applications.GoodsReceives;
using GodwitWHMS.Applications.InventoryTransactions;
using GodwitWHMS.Applications.NumberSequences;
using GodwitWHMS.Domain.DTOs;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{

    public class GoodsReceiveItemChildController : ODataController
    {

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<InventoryTransaction, GoodsReceiveItemChildDto>();
                CreateMap<GoodsReceiveItemChildDto, InventoryTransaction>();
            }
        }

        private readonly NumberSequenceService _numberSequenceService;
        private readonly GoodsReceiveService _goodsReceiveService;
        private readonly InventoryTransactionService _inventoryTransactionService;
        private readonly IMapper _mapper;

        public GoodsReceiveItemChildController(
            NumberSequenceService numberSequenceService,
            GoodsReceiveService goodsReceiveService,
            IMapper mapper,
            InventoryTransactionService inventoryTransactionService)
        {
            _mapper = mapper;
            _goodsReceiveService = goodsReceiveService;
            _inventoryTransactionService = inventoryTransactionService;
            _numberSequenceService = numberSequenceService;
        }

        [EnableQuery]
        public IQueryable<GoodsReceiveItemChildDto> Get()
        {
            const string HeaderKeyName = "ParentId";
            Request.Headers.TryGetValue(HeaderKeyName, out var headerValue);
            var parentId = int.Parse(headerValue.ToString());

            var moduleName = nameof(GoodsReceive) ?? string.Empty;

            return _inventoryTransactionService
                .GetAll()
                .Where(x => x.ModuleId == parentId && x.ModuleName == moduleName)
                .Select(x => _mapper.Map<GoodsReceiveItemChildDto>(x));
        }


        [EnableQuery]
        [HttpGet("{key}")]
        public SingleResult<GoodsReceiveItemChildDto> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_inventoryTransactionService
                .GetAll()
                .Where(x => x.Id == key)
            .Select(x => _mapper.Map<GoodsReceiveItemChildDto>(x)));
        }



        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<GoodsReceiveItemChildDto> delta)
        {
            try
            {
                var moduleName = nameof(GoodsReceive) ?? string.Empty;
                var child = await _inventoryTransactionService
                    .GetAll()
                    .Where(x => x.Id == key && x.ModuleName == moduleName)
                    .FirstOrDefaultAsync();

                if (child == null)
                {
                    return NotFound();
                }

                var dto = _mapper.Map<GoodsReceiveItemChildDto>(child);
                delta.Patch(dto);
                var entity = _mapper.Map(dto, child);
                await _inventoryTransactionService.UpdateAsync(entity);

                return Ok(_mapper.Map<GoodsReceiveItemChildDto>(entity));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GoodsReceiveItemChildDto postInput)
        {
            try
            {

                const string HeaderKeyName = "ParentId";
                Request.Headers.TryGetValue(HeaderKeyName, out var headerValue);
                var parentId = int.Parse(headerValue.ToString());
                var moduleName = nameof(GoodsReceive) ?? string.Empty;


                var entity = _mapper.Map<InventoryTransaction>(postInput);

                var parent = await _goodsReceiveService.GetByIdAsync(parentId);
                if (parent != null)
                {
                    entity.ModuleId = parent.Id;
                    entity.ModuleName = moduleName;
                    entity.ModuleCode = "GR";
                    entity.ModuleNumber = parent.Number ?? string.Empty;
                    entity.MovementDate = parent.ReceiveDate!.Value;
                    entity.Status = (InventoryTransactionStatus)parent.Status!;
                }

                entity.Number = _numberSequenceService.GenerateNumber(nameof(InventoryTransaction), "", "IVT");
                await _inventoryTransactionService.AddAsync(entity);

                var dto = _mapper.Map<InventoryTransaction>(entity);
                return Created("GoodsReceiveItemChild", dto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                var child = await _inventoryTransactionService.GetAll()
                    .Where(x => x.Id == key)
                    .FirstOrDefaultAsync();

                if (child == null)
                {
                    return BadRequest();
                }

                await _inventoryTransactionService.DeleteByIdAsync(child.Id);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
