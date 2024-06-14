using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodwitWHMS.Domain.DTOs;
public class PackageSkuDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ScannedCode { get; set; }
    public Guid? RowGuid { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
}