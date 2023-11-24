using Discord;

namespace Quoter.Commands.Abstractions;

public interface ICommandRegistration
{
    public string Name { get; }
    public string Description { get; }
    public IEnumerable<CommandOption> Options { get; }
    public Type CommandType { get; }
}

public class CommandOption
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsRequired { get; set; }
    public ApplicationCommandOptionType Type { get; set; }
}