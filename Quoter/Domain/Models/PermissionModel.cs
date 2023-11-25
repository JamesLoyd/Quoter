namespace Quoter.Domain.Models;

public class PermissionModel
{
    public ulong RoleId { get; set; }
    public string RoleName { get; set; } = "";
    public bool CanPurge { get; set; }
}