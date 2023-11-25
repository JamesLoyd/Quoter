using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.AddPermission;

public class AddPermissionCommand : ICommand<Result<Response>>
{
    public GuildModel Guild { get; set; }
    public PermissionModel PermissionModel { get; set; }
}