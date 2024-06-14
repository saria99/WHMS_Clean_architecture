using AutoMapper;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Formatter;
using GodwitWHMS.Applications.Features.PackagesSku;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class PackageSkuController : ODataController
    {
        private readonly PackageSkuService _packageSkuService;
        private readonly IMapper _mapper;

        public PackageSkuController(PackageSkuService packageSkuService, IMapper mapper)
        {
            _packageSkuService = packageSkuService;
            _mapper = mapper;
        }

        [EnableQuery]
        public IQueryable<PackageSkuDto> Get()
        {
            return _packageSkuService
                .GetAll()
                .Select(rec => new PackageSkuDto
                {
                    Id = rec.Id,
                    RowGuid = rec.RowGuid,
                    Code = rec.Code,
                    ScannedCode = rec.ScannedCode,
                    CreatedAtUtc = rec.CreatedAtUtc
                });
        }
    }
}
