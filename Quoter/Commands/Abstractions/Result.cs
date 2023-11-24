namespace Quoter.Commands.Abstractions;

public class Result<TValue> : Result
{
    private readonly TValue _value;

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error) =>
        _value = value;

    protected internal Result(TValue value, bool isSuccess, List<Error> errors)
        : base(isSuccess, errors) =>
        _value = value;


    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}

public class Result
{
    
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    protected internal Result(bool isSuccess, List<Error> errors)
    {
        switch (isSuccess)
        {
            case true when errors.Any(x => x != Error.None):
                throw new InvalidOperationException();
            case false when errors.Any(x => x == Error.None):
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Errors= errors;
                break;
        }
    }

    public void AddError(Error error)
    {
        Errors.Add(error);
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; set; }
    public List<Error> Errors { get; set; }

    public static Result Success() => new(true, Error.None);


    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);


    public static Result Failure(List<Error> errors) => new(false, errors);

    public static Result<TValue?> Failure<TValue>(List<Error> errors) => new(default, false, errors);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result Create(bool condition) => condition ? Success() : Failure(Error.ConditionNotMet);

    public static Result<TValue> Create<TValue>(TValue value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}
