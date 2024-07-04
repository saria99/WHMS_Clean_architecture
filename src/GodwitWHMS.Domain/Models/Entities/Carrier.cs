using GodwitWHMS.Domain.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class Carrier : _Base
    {
        public required string CarrierName { get; set; }

    }
}
