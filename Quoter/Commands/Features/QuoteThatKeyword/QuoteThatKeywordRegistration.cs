using Discord;
using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordRegistration : ICommandRegistration
{
    public string Name => "q-k";
    public string Description => "Quote a quote by a keyword";

    public IEnumerable<CommandOption> Options => new List<CommandOption>
    {
        new()
        {
            Name = "quote",
            Type = ApplicationCommandOptionType.String,
            Description = "The quote to quote",
            IsRequired = true
        },
        new()
        {
            Name = "keyword",
            Type = ApplicationCommandOptionType.String,
            Description = "The keyword to quote",
            IsRequired = true
        }
    };

    public Type CommandType  => typeof(QuoteThatKeywordCommand);
}