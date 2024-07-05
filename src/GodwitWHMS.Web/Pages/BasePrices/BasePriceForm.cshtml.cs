using AutoMapper;
using GodwitWHMS.Infrastructures.Countries;
using GodwitWHMS.Infrastructures.Extensions;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Applications.Features.Carriers;
using GodwitWHMS.Applications.Features.Countries;

namespace GodwitWHMS.Pages.BasePrices
{
    [Authorize]
    public class BasePriceFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly BasePriceService _basePriceService;
        private readonly CarrierService _carrierService;
        private readonly CountryServicev2 _countryService;

        public BasePriceFormModel(
            IMapper mapper,
            BasePriceService basePriceService,
            CarrierService carrierService,
            CountryServicev2 countryService)
        {
            _mapper = mapper;
            _basePriceService = basePriceService;
            _carrierService = carrierService;
            _countryService = countryService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public BasePriceModel BasePriceForm { get; set; } = default!;

        public class BasePriceModel
        {
            public int? Id { get; set; }
            public Guid? RowGuid { get; set; }

            [DisplayName("Carrier")]
            public int CarrierId { get; set; }

            [DisplayName("Origin Country")]
            public int OriginCountryId { get; set; }

            [DisplayName("Destination Country")]
            public int DestinationCountryId { get; set; }

            [DisplayName("Weight")]
            public decimal Weight { get; set; }

            [DisplayName("Price")]
            public decimal Price { get; set; }

            [DisplayName("Price with Fuel Surcharge")]
            public decimal PriceWithFuelSurcharge { get; set; }

            [DisplayName("Total Price")]
            public decimal TotalPrice { get; set; }

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
                CreateMap<BasePrice, BasePriceModel>();
                CreateMap<BasePriceModel, BasePrice>();
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
                var existing = await _basePriceService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                BasePriceForm = _mapper.Map<BasePriceModel>(existing);
            }
            else
            {
                BasePriceForm = new BasePriceModel
                {
                    RowGuid = Guid.Empty
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(BasePriceForm))] BasePriceModel input)
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
                var newobj = _mapper.Map<BasePrice>(input); // Assuming "Express" for example

                await _basePriceService.AddAsync(newobj);

                this.WriteStatusMessage($"Success create new data.");
                return Redirect($"./BasePriceForm?rowGuid={newobj.RowGuid}&action=edit");
            }
            else if (action == "edit")
            {
                var existing = await _basePriceService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                _mapper.Map(input, existing);

                await _basePriceService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./BasePriceForm?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _basePriceService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _basePriceService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./BasePriceList");
            }
            return Page();
        }

        private decimal CalculateFuelSurcharge(decimal price, int carrierId)
        {
            // Implement your fuel surcharge calculation logic here
            return 0; // Replace with actual calculation
        }

        private decimal CalculateCommission(decimal priceWithFuelSurcharge, string serviceType)
        {
            // Implement your commission calculation logic here
            return 0; // Replace with actual calculation
        }
    }
}
