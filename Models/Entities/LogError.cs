using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Models.Entities
{
    public class LogError : _Base
    {
        public LogError() { }

        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? AdditionalInfo { get; set; }

    }
}
