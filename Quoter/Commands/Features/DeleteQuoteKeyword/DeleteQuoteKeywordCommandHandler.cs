using Microsoft.EntityFrameworkCore;
using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.DeleteQuoteKeyword;

public class DeleteQuoteKeywordCommandHandler : CommandHandler<DeleteQuoteKeywordCommand,Result<Response>>
{
    private readonly QuoterContext _quoterContext;

    public DeleteQuoteKeywordCommandHandler(QuoterContext quoterContext)
    {
        _quoterContext = quoterContext;
    }
    protected override async Task<Result<Response>> HandleCommandAsync(DeleteQuoteKeywordCommand command, CancellationToken cancellationToken = default)
    {
        var perms = await _quoterContext.Permissions.ToListAsync(cancellationToken: cancellationToken);
        foreach (var perm in perms)
        {
            var role = command.Guild.Roles.FirstOrDefault(x => x.Id.ToString() == perm.RoleId);
            if (role == null) continue;
            if (perm.CanPurge)
            {
                _quoterContext.Quotes.RemoveRange(_quoterContext.Quotes.Where(x =>
                    x.KeyWord == command.Keyword && x.GuildId == command.Guild.Id.ToString()));
                await _quoterContext.SaveChangesAsync(cancellationToken);
                return Result.Success(new Response
                {
                    Ephemeral = true,
                    Message = "Keyword purged"
                });
            }
        }

        return Result.Failure<Response>(new Error("500", "Failure to purge quote", true));
    }
}