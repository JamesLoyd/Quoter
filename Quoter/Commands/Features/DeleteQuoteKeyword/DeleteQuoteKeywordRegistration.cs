using Discord;
using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.DeleteQuoteKeyword;

public class DeleteQuoteKeywordRegistration : ICommandRegistration
{
    public string CommandName => "dq";
    public string Description => "Delete a quote keyword";

    public IEnumerable<CommandOption> Options => new List<CommandOption>
    {
        new CommandOption
        {
            Name = "Keyword",
            Description = "Keyword to delete",
            IsRequired = true,
            Type = ApplicationCommandOptionType.String
        }
    };

    public Type CommandType => typeof(DeleteQuoteKeywordCommand);
}