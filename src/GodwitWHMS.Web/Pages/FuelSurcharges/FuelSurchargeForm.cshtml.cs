using AutoMapper;
using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Applications.Features.Carriers;
using GodwitWHMS.Applications.Features.Countries;
using GodwitWHMS.Applications.Features.FuelSurcharges;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GodwitWHMS.Pages.FuelSurcharges
{
    [Authorize]
    public class FuelSurchargeFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly FuelSurchargeService _fuelSurchargeService;
        private readonly CarrierService _carrierService;
        private readonly CountryServicev2 _countryService;

        public FuelSurchargeFormModel(
            IMapper mapper,
            FuelSurchargeService fuelSurchargeService,
            CarrierService carrierService,
            CountryServicev2 countryService)
        {
            _mapper = mapper;
            _fuelSurchargeService = fuelSurchargeService;
            _carrierService = carrierService;
            _countryService = countryService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public FuelSurchargeModel FuelSurchargeForm { get; set; } = default!;

        public class FuelSurchargeModel
        {
            public int? Id { get; set; }
            public Guid? RowGuid { get; set; }

            [DisplayName("Carrier")]
            public int CarrierId { get; set; }

            [DisplayName("Origin Country")]
            public int OriginCountryId { get; set; }

            [DisplayName("Destination Country")]
            public int DestinationCountryId { get; set; }

            [DisplayName("Effective Date")]
            public DateTime EffectiveDate { get; set; }

            [DisplayName("Fuel Surcharge (%)")]
            public decimal FuelSurchargePercentage { get; set; }

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
                CreateMap<FuelSurcharge, FuelSurchargeModel>();
                CreateMap<FuelSurchargeModel, FuelSurcharge>();
            }
        }

        public ICollection<SelectListItem> CarrierLookup { get; set; } = default!;
        public ICollection<SelectListItem> CountryLookup { get; set; } = default!;

        private void BindLookup()
        {
            CarrierLookup = _carrierService
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CarrierName
                }).ToList();

            CountryLookup = _countryService
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CountryName
                }).ToList();
        }

        public async Task OnGetAsync(Guid? rowGuid)
        {
            this.SetupViewDataTitleFromUrl();
            this.SetupStatusMessage();
            StatusMessage = this.ReadStatusMessage();

            var action = Request.Query["action"];
            Action = action;

            BindLookup();

            if (rowGuid.HasValue)
            {
                var existing = await _fuelSurchargeService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                FuelSurchargeForm = _mapper.Map<FuelSurchargeModel>(existing);
            }
            else
            {
                FuelSurchargeForm = new FuelSurchargeModel
                {
                    RowGuid = Guid.Empty
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(FuelSurchargeForm))] FuelSurchargeModel input)
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
                var newobj = _mapper.Map<FuelSurcharge>(input);
                await _fuelSurchargeService.AddAsync(newobj);

                this.WriteStatusMessage($"Success create new data.");
                return Redirect($"./FuelSurchargeForm?id={newobj.RowGuid}&action=edit");
            }
            else if (action == "edit")
            {
                var existing = await _fuelSurchargeService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                _mapper.Map(input, existing);
                await _fuelSurchargeService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./FuelSurchargeForm?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _fuelSurchargeService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _fuelSurchargeService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./FuelSurchargeList");
            }
            return Page();
        }
    }
}
