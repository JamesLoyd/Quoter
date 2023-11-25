using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.QuoteThat;

public class QuoteThatCommandHandler : CommandHandler<QuoteThatCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(QuoteThatCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}