using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GodwitWHMS.Applications.LogSessions
{
    public class LogSessionService : Repository<LogSession>
    {
        public LogSessionService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }

        public async Task CollectLoginSessionDataAsync()
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            var data = new LogSession
            {
                UserId = userId,
                UserName = userName,
                IPAddress = ipAddress,
                Action = "Login"
            };

            await AddAsync(data);

            var entity = await _context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (entity != null)
            {
                entity.IsOnline = true;
                _context.Set<ApplicationUser>().Update(entity);
                await _context.SaveChangesAsync();
            }


        }

        public async Task CollectLogoutSessionDataAsync()
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            var data = new LogSession
            {
                UserId = userId,
                UserName = userName,
                IPAddress = ipAddress,
                Action = "Logout"
            };

            await AddAsync(data);

            var entity = await _context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (entity != null)
            {
                entity.IsOnline = false;
                _context.Set<ApplicationUser>().Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public void PurgeAllData()
        {
            _context.LogSession.RemoveRange(_context.LogSession);
            _context.SaveChanges();
        }
    }
}
