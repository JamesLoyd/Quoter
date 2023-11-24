using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.ListKeywords;

public class ListKeywordsRegistration : ICommandRegistration
{
    public string Name => "lk";
    public string Description => "List all keywords";
    public IEnumerable<CommandOption> Options { get; } = null!;
    public Type CommandType => typeof(ListKeywordQuery);
}