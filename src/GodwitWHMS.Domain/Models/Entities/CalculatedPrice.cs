using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Domain.Models.Entities;

public class CalculatedPrice : _Base
{
    public CalculatedPrice() { }

    public required int BasePriceId { get; set; }
    public BasePrice? BasePrice { get; set; }

    public ServiceType ServiceType { get; set; }

    public decimal PriceWithFuelSurcharge { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsCheapest { get; set; }
}


public enum ServiceType
{
    Express,
    Economy
}
