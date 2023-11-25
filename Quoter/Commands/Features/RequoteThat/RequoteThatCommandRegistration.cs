using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.RequoteThat;

public class RequoteThatCommandRegistration: ICommandRegistration
{
    public string CommandName { get; }
    public string Description { get; }
    public IEnumerable<CommandOption> Options { get; }
    public Type CommandType { get; }
}