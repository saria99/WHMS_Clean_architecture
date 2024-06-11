using System.ComponentModel;

namespace GodwitWHMS.Domain.Models.Enums
{
    public enum InventoryTransType
    {
        [Description("In")]
        In = 1,
        [Description("Out")]
        Out = -1,
    }
}
