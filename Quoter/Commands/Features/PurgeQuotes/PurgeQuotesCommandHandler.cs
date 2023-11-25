using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.PurgeQuotes;

public class PurgeQuotesCommandHandler : CommandHandler<PurgeQuotesCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(PurgeQuotesCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}