using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.PurgeQuote;

public class PurgeQuoteCommandHandler : CommandHandler<PurgeQuoteCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(PurgeQuoteCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}