using GodwitWHMS.Models.Contracts;
using GodwitWHMS.Models.Enums;

namespace GodwitWHMS.Models.Entities
{
    public class TransferIn : _Base
    {
        public TransferIn() { }
        public string? Number { get; set; }
        public DateTime? TransferReceiveDate { get; set; }
        public TransferStatus? Status { get; set; }
        public string? Description { get; set; }
        public required int TransferOutId { get; set; }
        public TransferOut? TransferOut { get; set; }
    }
}
