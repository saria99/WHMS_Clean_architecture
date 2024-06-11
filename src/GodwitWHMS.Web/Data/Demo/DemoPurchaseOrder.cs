using GodwitWHMS.Applications.NumberSequences;
using GodwitWHMS.Applications.Products;
using GodwitWHMS.Applications.PurchaseOrderItems;
using GodwitWHMS.Applications.PurchaseOrders;
using GodwitWHMS.Applications.Taxes;
using GodwitWHMS.Applications.Vendors;
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Domain.Models.Enums;

namespace GodwitWHMS.Data.Demo
{
    public static class DemoPurchaseOrder
    {
        public static async Task GenerateAsync(IServiceProvider services)
        {
            var purchaseOrderService = services.GetRequiredService<PurchaseOrderService>();
            var purchaseOrderItemService = services.GetRequiredService<PurchaseOrderItemService>();
            var vendorService = services.GetRequiredService<VendorService>();
            var taxSerice = services.GetRequiredService<TaxService>();
            var productService = services.GetRequiredService<ProductService>();
            var numberSequenceService = services.GetRequiredService<NumberSequenceService>();

            Random random = new Random();
            int orderStatusLength = Enum.GetNames(typeof(PurchaseOrderStatus)).Length;
            var vendors = vendorService.GetAll().Select(x => x.Id).ToArray();
            var taxes = taxSerice.GetAll().Select(x => x.Id).ToArray();
            var products = productService.GetAll().ToList();

            var dateFinish = DateTime.Now;
            var dateStart = new DateTime(dateFinish.AddMonths(-12).Year, dateFinish.AddMonths(-12).Month, 1);

            for (DateTime date = dateStart; date < dateFinish; date = date.AddMonths(1))
            {
                DateTime[] transactionDates = DbInitializer.GetRandomDays(date.Year, date.Month, 6);

                foreach (DateTime transDate in transactionDates)
                {
                    var purchaseOrder = new PurchaseOrder
                    {
                        Number = numberSequenceService.GenerateNumber(nameof(PurchaseOrder), "", "PO"),
                        OrderDate = transDate,
                        OrderStatus = (PurchaseOrderStatus)random.Next(0, orderStatusLength),
                        VendorId = DbInitializer.GetRandomValue(vendors, random),
                        TaxId = DbInitializer.GetRandomValue(taxes, random),
                    };
                    await purchaseOrderService.AddAsync(purchaseOrder);

                    int numberOfProducts = random.Next(3, 6);
                    for (int i = 0; i < numberOfProducts; i++)
                    {
                        var product = products[random.Next(0, products.Count())];
                        var purchaseOrderItem = new PurchaseOrderItem
                        {
                            PurchaseOrderId = purchaseOrder.Id,
                            ProductId = product.Id,
                            Summary = product.Number,
                            UnitPrice = product.UnitPrice,
                            Quantity = random.Next(20, 50),
                        };
                        purchaseOrderItem.RecalculateTotal();
                        await purchaseOrderItemService.AddAsync(purchaseOrderItem);
                    }


                }
            }
        }
    }
}
