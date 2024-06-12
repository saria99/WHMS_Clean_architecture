using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using System.Security.Claims;
using UAParser;
using Microsoft.AspNetCore.Http;
using DeviceDetectorNET;

namespace GodwitWHMS.Applications.LogAnalytics
{
    public class LogAnalyticService : Repository<LogAnalytic>
    {
        public LogAnalyticService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }

        public async Task CollectAnalyticDataAsync()
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userAgentString = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"];
            var userIpAddress = _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var url = _httpContextAccessor?.HttpContext?.Request.Path;
            var queryString = _httpContextAccessor?.HttpContext?.Request.QueryString;

            var deviceDetector = new DeviceDetector(userAgentString);
            deviceDetector.Parse();
            var deviceType = deviceDetector.GetDeviceName();

            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgentString);
            var browserName = clientInfo?.UA?.Family;
            var browserVersion = clientInfo?.UA?.Major;

            var logAnalytic = new LogAnalytic
            {
                UserId = userId,
                UserName = userName,
                IPAddress = userIpAddress,
                Url = url + queryString,
                Device = deviceType,
                GeographicLocation = "",
                Browser = $"{browserName} {browserVersion}"
            };

            await AddAsync(logAnalytic);
        }

        public void PurgeAllData()
        {
            _context.LogAnalytic.RemoveRange(_context.LogAnalytic);
            _context.SaveChanges();
        }

    }
}
