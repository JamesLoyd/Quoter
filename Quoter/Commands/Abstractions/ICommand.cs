using MediatR;
using Quoter.Domain.Interfaces;

namespace Quoter.Commands.Abstractions;

public interface ICommand : ICommand<Unit>
{
}

public partial interface ICommand<out T> : IRequest<T>, IHasUser
    where T : notnull
{
}

public partial interface ICommand<out T> : IRequest<T>, IHasUser, IHasGuild
    where T : notnull
{
}

public partial interface ICommand<out T> : IRequest<T>, IHasUser, IHasGuild, IHasChannel
    where T : notnull
{
}