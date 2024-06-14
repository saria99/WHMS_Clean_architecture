using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities;

public class PackageSku: _Base
{
    public PackageSku() { }
    public int Id { get; set; }
    public string Code { get; set; }
    public string ScannedCode { get; set; }
}