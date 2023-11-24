using MediatR;

namespace Quoter;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>, IHasUser
    where T : notnull
{
}