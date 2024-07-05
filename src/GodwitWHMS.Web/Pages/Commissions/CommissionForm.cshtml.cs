using AutoMapper;
using GodwitWHMS.Applications.Features.Commissions;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GodwitWHMS.Pages.Commissions
{
    [Authorize]
    public class CommissionFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly CommissionService _commissionService;

        public CommissionFormModel(IMapper mapper, CommissionService commissionService)
        {
            _mapper = mapper;
            _commissionService = commissionService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        [BindProperty]
        public CommissionModel CommissionForm { get; set; } = default!;

        public class CommissionModel
        {
            public int? Id { get; set; }
            public Guid? RowGuid { get; set; }

            [DisplayName("Service Type")]
            public string ServiceType { get; set; } = string.Empty;

            [DisplayName("Commission Percentage")]
            public decimal CommissionPercentage { get; set; }

            [DisplayName("Effective Date")]
            public DateTime EffectiveDate { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Commission, CommissionModel>();
                CreateMap<CommissionModel, Commission>();
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
                var existing = await _commissionService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                CommissionForm = _mapper.Map<CommissionModel>(existing);
            }
            else
            {
                CommissionForm = new CommissionModel
                {
                    EffectiveDate = DateTime.Now
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(CommissionForm))] CommissionModel input)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                throw new Exception(message);
            }

            var existing = await _commissionService.GetByRowGuidAsync(input.RowGuid);
            if (existing == null)
            {
                throw new Exception($"Unable to load existing data: {input.RowGuid}");
            }

            _mapper.Map(input, existing);
            await _commissionService.UpdateAsync(existing);

            this.WriteStatusMessage($"Success update existing data.");
            return Redirect($"./CommissionForm?rowGuid={existing.RowGuid}&action=edit");
        }
    }
}
