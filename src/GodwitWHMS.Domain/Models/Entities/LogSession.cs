using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Domain.Models.Entities
{
    public class LogSession : _Base
    {
        public LogSession() { }

        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? IPAddress { get; set; }
        public string? Action { get; set; }

    }
}
