using AutoMapper;
using GodwitWHMS.Applications.Features.PackagesSku;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.BarCode;
using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GodwitWHMS.Pages.PackageSkus
{
    [Authorize]
    public class GenerateSkuModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly PackageSkuService _skuService;
        private readonly IBarcodeGenerator _barcodeGenerator;

        public GenerateSkuModel(
            IMapper mapper,
            PackageSkuService skuService,
            IBarcodeGenerator barcodeGenerator)
        {
            _mapper = mapper;
            _skuService = skuService;
            _barcodeGenerator = barcodeGenerator;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public PackageSkuModel PackageSkuForm { get; set; } = default!;

        public class PackageSkuModel
        {
            public Guid? RowGuid { get; set; }
            public int? Id { get; set; }

            [DisplayName("Code")]
            public string Code { get; set; } = string.Empty;

            [DisplayName("Scanned Code")]
            public string? ScannedCode { get; set; }

            [DisplayName("Created At")]
            public string? CreatedAtString { get; set; } = string.Empty;

            [DisplayName("Created By")]
            public string? CreatedByUserName { get; set; } = string.Empty;

            [DisplayName("Updated At")]
            public string? UpdatedAtString { get; set; } = string.Empty;

            [DisplayName("Updated By")]
            public string? UpdatedByUserName { get; set; } = string.Empty;
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<PackageSku, PackageSkuModel>();
                CreateMap<PackageSkuModel, PackageSku>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Prevent modifying the primary key
            }
        }

        public async Task OnGetAsync(Guid? rowGuid)
        {
            this.SetupViewDataTitleFromUrl();
            this.SetupStatusMessage();
            StatusMessage = this.ReadStatusMessage();

            var action = Request.Query["action"];
            Action = action;

            if (rowGuid.HasValue)
            {
                var existing = await _skuService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                PackageSkuForm = _mapper.Map<PackageSkuModel>(existing);
            }
            else
            {
                PackageSkuForm = new PackageSkuModel
                {
                    RowGuid = Guid.Empty,
                    Id = 0
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(PackageSkuForm))] PackageSkuModel input, string? generateBarcode)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                throw new Exception(message);
            }

            var action = "create";

            if (!string.IsNullOrEmpty(Request.Query["action"]))
            {
                action = Request.Query["action"];
            }

            if (action == "create")
            {
                var newObj = _mapper.Map<PackageSku>(input);
                newObj.Code = _skuService.GenerateNumber(nameof(PackageSku), "GW-", "", false, 8);

                if (string.IsNullOrEmpty(newObj.Code))
                {
                    ModelState.AddModelError(nameof(PackageSkuForm.ScannedCode), "Scanned code is required to generate barcode.");
                    return Page();
                }

                var barcode = _barcodeGenerator.GenerateBarcode(input.ScannedCode, 31, 23);
                // return File(barcode, "image/png");

                await _skuService.AddAsync(newObj);

                this.WriteStatusMessage($"Success create new data.");
                return Redirect($"./GenerateSku?rowGuid={newObj.RowGuid}&action=edit");
            }
            else if (action == "edit")
            {
                var existing = await _skuService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                // Ignore the Id during mapping to prevent modifying the primary key
                _mapper.Map(input, existing);
                await _skuService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./GenerateSku?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _skuService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _skuService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./PackageSkuList");
            }
            return Page();
        }
    }
}
