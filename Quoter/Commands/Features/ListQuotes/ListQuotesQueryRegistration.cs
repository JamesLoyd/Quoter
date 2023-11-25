using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.ListQuotes;

public class ListQuotesQueryRegistration : ICommandRegistration
{
    public string CommandName { get; }
    public string Description { get; }
    public IEnumerable<CommandOption> Options { get; }
    public Type CommandType { get; }
}