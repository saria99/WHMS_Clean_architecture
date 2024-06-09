using Microsoft.AspNetCore.Mvc.Rendering;

namespace GodwitWHMS.Infrastructures.Countries
{
    public interface ICountryService
    {
        ICollection<SelectListItem> GetCountries();
    }
}
