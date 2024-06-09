using Microsoft.AspNetCore.Mvc.Rendering;

namespace GodwitWHMS.Infrastructures.Currencies
{
    public interface ICurrencyService
    {
        ICollection<SelectListItem> GetCurrencies();
    }
}
