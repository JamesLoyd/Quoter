using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordCommandHandler : CommandHandler<QuoteThatKeywordCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(QuoteThatKeywordCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}