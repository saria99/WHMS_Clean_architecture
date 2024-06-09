namespace GodwitWHMS.DTOs
{
    public class AspNetRoleChildDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Permission { get; set; }
        public string? Module { get; set; }
        public bool GrantAccess { get; set; }
    }
}
