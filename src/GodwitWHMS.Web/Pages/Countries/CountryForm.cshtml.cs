using AutoMapper;
using GodwitWHMS.Applications.Features.Countries;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace GodwitWHMS.Pages.Countries
{
    public class CountryFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly CountryServicev2 _countryService;

        public CountryFormModel(
            IMapper mapper,
            CountryServicev2 countryService)
        {
            _mapper = mapper;
            _countryService = countryService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public CountryModel CountryForm { get; set; } = default!;

        public class CountryModel
        {
            public Guid? RowGuid { get; set; }
            public int? Id { get; set; }

            [DisplayName("Country Code")]
            public string CountryCode { get; set; } = string.Empty;

            [DisplayName("Country Name")]
            public string CountryName { get; set; } = string.Empty;

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
                CreateMap<Country, CountryModel>();
                CreateMap<CountryModel, Country>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Prevent modifying the primary key
            }
        }

        private void BindLookup()
        {

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
                var existing = await _countryService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                CountryForm = _mapper.Map<CountryModel>(existing);
            }
            else
            {
                CountryForm = new CountryModel
                {
                    RowGuid = Guid.Empty,
                    Id = 0
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(CountryForm))] CountryModel input)
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
                var newObj = _mapper.Map<Country>(input);
                await _countryService.AddAsync(newObj);

                this.WriteStatusMessage($"Success create new data.");
                return Redirect($"./CountryForm?rowGuid={newObj.RowGuid}&action=edit");
            }
            else if (action == "edit")
            {
                var existing = await _countryService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                // Ignore the Id during mapping to prevent modifying the primary key
                _mapper.Map(input, existing);
                await _countryService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./CountryForm?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _countryService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _countryService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./CountryList");
            }
            return Page();
        }
    }
}
