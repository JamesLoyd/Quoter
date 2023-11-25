using Discord;
using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.AddPermission;

public class AddPermissionCommandRegistration: ICommandRegistration
{
    public string CommandName => "add-perm";
    public string Description => "The role you want to assign";

    public IEnumerable<CommandOption> Options => new List<CommandOption>
    {
        new CommandOption
        {
            Description = "Can Purge quotes",
            Name = "Permission",
            Type = ApplicationCommandOptionType.Boolean,
            IsRequired = true
        },
        new CommandOption
        {
            Name = "Role",
            Description = "The role you want to add",
            Type = ApplicationCommandOptionType.Role,
            IsRequired = true
        }
    };

    public Type CommandType => typeof(AddPermissionCommand);
}