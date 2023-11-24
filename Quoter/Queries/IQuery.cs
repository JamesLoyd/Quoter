using MediatR;

namespace Quoter.Queries;

public interface IQuery<out T> : IRequest<T>
    where T : notnull { }