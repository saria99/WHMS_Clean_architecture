using Microsoft.AspNetCore.Mvc.Rendering;

namespace GodwitWHMS.Infrastructures.TimeZones
{
    public interface ITimeZoneService
    {
        ICollection<SelectListItem> GetAllTimeZones();
    }
}
