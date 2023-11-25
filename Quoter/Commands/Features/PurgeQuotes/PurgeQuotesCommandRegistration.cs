using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.PurgeQuotes;

public class PurgeQuotesCommandRegistration: ICommandRegistration
{
    public string CommandName { get; }
    public string Description { get; }
    public IEnumerable<CommandOption> Options { get; }
    public Type CommandType { get; }
}