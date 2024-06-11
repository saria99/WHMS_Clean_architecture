using System.ComponentModel;

namespace GodwitWHMS.Domain.Models.Enums
{
    public enum UserType
    {
        [Description("Internal")]
        Internal = 0,
        [Description("Customer")]
        Customer = 1,
        [Description("Vendor")]
        Vendor = 2
    }
}
