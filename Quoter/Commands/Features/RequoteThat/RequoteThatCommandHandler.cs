using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.RequoteThat;

public class RequoteThatCommandHandler : CommandHandler<RequoteThatCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(RequoteThatCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}