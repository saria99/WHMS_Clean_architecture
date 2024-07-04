using AutoMapper;
using GodwitWHMS.Applications.Features.Carriers;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GodwitWHMS.Pages.Carriers
{
    [Authorize]
    public class CarrierFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly CarrierService _carrierService;
        public CarrierFormModel(
            IMapper mapper,
            CarrierService carrierService)
        {
            _mapper = mapper;
            _carrierService = carrierService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public CarrierModel CarrierForm { get; set; } = default!;

        public class CarrierModel
        {
            public int? Id { get; set; }
            public Guid? RowGuid { get; set; }

            [DisplayName("Carrier Name")]
            public string? CarrierName { get; set; } = string.Empty;

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
                CreateMap<Carrier, CarrierModel>();
                CreateMap<CarrierModel, Carrier>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
            }
        }

        private void BindLookup()
        {
            // Add any lookup binding if needed
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
                var existing = await _carrierService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                CarrierForm = _mapper.Map<CarrierModel>(existing);
            }
            else
            {
                CarrierForm = new CarrierModel
                {
                    RowGuid = Guid.Empty
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(CarrierForm))] CarrierModel input)
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
                var newObj = _mapper.Map<Carrier>(input);
                await _carrierService.AddAsync(newObj);

                var createdCarrier = await _carrierService.GetByRowGuidAsync(newObj.RowGuid);

                if (createdCarrier != null)
                {
                    this.WriteStatusMessage($"Success create new data.");
                    return Redirect($"./CarrierForm?rowGuid={createdCarrier.RowGuid}&action=edit");
                }
                else
                {
                    throw new Exception("Failed to create new carrier");
                }
            }
            else if (action == "edit")
            {
                var existing = await _carrierService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                _mapper.Map(input, existing);
                await _carrierService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./CarrierForm?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _carrierService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _carrierService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./CarrierList");
            }
            return Page();
        }
    }
}
