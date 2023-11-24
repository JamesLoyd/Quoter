using MediatR;
using Quoter.Domain;

namespace Quoter.Commands;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>, IHasUser
    where T : notnull
{
}