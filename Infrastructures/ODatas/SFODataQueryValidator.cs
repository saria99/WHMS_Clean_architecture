using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;

namespace GodwitWHMS.Infrastructures.ODatas
{
    public class SFODataQueryValidator : ODataQueryValidator
    {
        public override void Validate(ODataQueryOptions options, ODataValidationSettings validationSettings)
        {
            validationSettings.MaxNodeCount = 300;
            base.Validate(options, validationSettings);
        }
    }
}
