using AutoMapper;
using GodwitWHMS.Applications.NumberSequences;
using GodwitWHMS.Applications.Products;
using GodwitWHMS.Applications.PurchaseOrders;
using GodwitWHMS.Applications.Taxes;
using GodwitWHMS.Applications.Vendors;
using GodwitWHMS.Infrastructures.Extensions;
using GodwitWHMS.Models.Entities;
using GodwitWHMS.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace GodwitWHMS.Pages.PurchaseOrders
{
    [Authorize]
    public class PurchaseOrderFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly NumberSequenceService _numberSequenceService;
        private readonly VendorService _vendorService;
        private readonly TaxService _taxService;
        private readonly ProductService _productService;
        public PurchaseOrderFormModel(
            IMapper mapper,
            PurchaseOrderService purchaseOrderService,
            NumberSequenceService numberSequenceService,
            VendorService vendorService,
            TaxService taxService,
            ProductService productService
            )
        {
            _mapper = mapper;
            _purchaseOrderService = purchaseOrderService;
            _numberSequenceService = numberSequenceService;
            _taxService = taxService;
            _vendorService = vendorService;
            _productService = productService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;

        [BindProperty]
        public PurchaseOrderModel PurchaseOrderForm { get; set; } = default!;

        public class PurchaseOrderModel
        {
            public Guid? RowGuid { get; set; }
            public int? Id { get; set; }

            [DisplayName("Order Date")]
            public DateTime OrderDate { get; set; } = DateTime.Now;

            [DisplayName("Order Status")]
            public PurchaseOrderStatus OrderStatus { get; set; }

            [DisplayName("Description")]
            public string? Description { get; set; }

            [DisplayName("Vendor")]
            public int VendorId { get; set; }

            [DisplayName("Tax")]
            public int TaxId { get; set; }

            [DisplayName("Before Tax Amount")]
            public double? BeforeTaxAmount { get; set; }

            [DisplayName("Tax Amount")]
            public double? TaxAmount { get; set; }

            [DisplayName("After Tax Amount")]
            public double? AfterTaxAmount { get; set; }

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
                CreateMap<PurchaseOrder, PurchaseOrderModel>();
                CreateMap<PurchaseOrderModel, PurchaseOrder>();
            }
        }

        public ICollection<SelectListItem> VendorLookup { get; set; } = default!;
        public ICollection<SelectListItem> TaxLookup { get; set; } = default!;
        public ICollection<object> ProductLookup { get; set; } = default!;
        public ICollection<object> PriceLookup { get; set; } = default!;
        public ICollection<object> NumberLookup { get; set; } = default!;
        private void BindLookup()
        {

            VendorLookup = _vendorService.GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name}"
            }).ToList();

            TaxLookup = _taxService.GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name}"
            }).ToList();


            ProductLookup = _productService.GetAll()
                .Select(x => new { ProductId = x.Id, ProductName = $"{x.Name} / {x.UnitPrice}" } as object)
                .ToList();

            PriceLookup = _productService.GetAll()
                .Select(x => new { ProductId = x.Id, ProductPrice = x.UnitPrice } as object)
                .ToList();

            NumberLookup = _productService.GetAll()
                .Select(x => new { ProductId = x.Id, ProductNumber = x.Number } as object)
                .ToList();


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
                var existing = await _purchaseOrderService.GetByRowGuidAsync(rowGuid);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {rowGuid}");
                }
                PurchaseOrderForm = _mapper.Map<PurchaseOrderModel>(existing);
                Number = existing.Number ?? string.Empty;
            }
            else
            {
                PurchaseOrderForm = new PurchaseOrderModel
                {
                    RowGuid = Guid.Empty,
                    Id = 0
                };
            }
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = nameof(PurchaseOrderForm))] PurchaseOrderModel input)
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
                var newobj = _mapper.Map<PurchaseOrder>(input);

                Number = _numberSequenceService.GenerateNumber(nameof(PurchaseOrder), "", "PO");
                newobj.Number = Number;

                await _purchaseOrderService.AddAsync(newobj);

                this.WriteStatusMessage($"Success create new data.");
                return Redirect($"./PurchaseOrderForm?rowGuid={newobj.RowGuid}&action=edit");
            }
            else if (action == "edit")
            {
                var existing = await _purchaseOrderService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                _mapper.Map(input, existing);
                await _purchaseOrderService.UpdateAsync(existing);

                this.WriteStatusMessage($"Success update existing data.");
                return Redirect($"./PurchaseOrderForm?rowGuid={existing.RowGuid}&action=edit");
            }
            else if (action == "delete")
            {
                var existing = await _purchaseOrderService.GetByRowGuidAsync(input.RowGuid);
                if (existing == null)
                {
                    var message = $"Unable to load existing data: {input.RowGuid}";
                    throw new Exception(message);
                }

                await _purchaseOrderService.DeleteByRowGuidAsync(input.RowGuid);

                this.WriteStatusMessage($"Success delete existing data.");
                return Redirect("./PurchaseOrderList");
            }
            return Page();
        }

    }
}
