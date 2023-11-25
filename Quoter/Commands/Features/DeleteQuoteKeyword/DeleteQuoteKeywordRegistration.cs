using Discord;
using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.DeleteQuoteKeyword;

public class DeleteQuoteKeywordRegistration : ICommandRegistration
{
    public string CommandName => "delete-keyword-2";
    public string Description => "Delete a quote keyword";

    public IEnumerable<CommandOption> Options => new List<CommandOption>
    {
        new CommandOption
        {
            Name = "keyword",
            Description = "Keyword to delete",
            IsRequired = true,
            Type = ApplicationCommandOptionType.String
        }
    };

    public Type CommandType => typeof(DeleteQuoteKeywordCommand);

    public bool Defer => true;
    public bool IsEphemeral => true;

}