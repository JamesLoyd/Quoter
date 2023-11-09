using System.ComponentModel.DataAnnotations;

namespace Quoter.Entities;

public class Permission
{
    public Guid Id { get; set; }
    public string RoleId { get; set; } = "";
    public string RoleName { get; set; } = "";
    public bool CanPurge { get; set; }
    public bool CanDeleteSingleQuote { get; set; }
}