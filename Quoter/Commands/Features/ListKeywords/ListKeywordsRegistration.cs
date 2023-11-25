using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.ListKeywords;

public class ListKeywordsRegistration : ICommandRegistration
{
    public string CommandName => "lk";
    public string Description => "List all keywords";
    public IEnumerable<CommandOption> Options => new List<CommandOption>();
    public Type CommandType => typeof(ListKeywordQuery);
}