﻿using GodwitWHMS.Applications.Taxes;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GodwitWHMS.ApiOData
{
    public class TaxController : ODataController
    {
        private readonly TaxService _taxService;

        public TaxController(TaxService taxService)
        {
            _taxService = taxService;
        }

        [EnableQuery]
        public IQueryable<TaxDto> Get()
        {
            return _taxService
                .GetAll()
                .Select(rec => new TaxDto
                {
                    Id = rec.Id,
                    RowGuid = rec.RowGuid,
                    Name = rec.Name,
                    Description = rec.Description,
                    CreatedAtUtc = rec.CreatedAtUtc,
                    Percentage = rec.Percentage,
                });
        }


    }
}
