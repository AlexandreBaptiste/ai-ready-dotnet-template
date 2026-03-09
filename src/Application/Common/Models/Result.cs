namespace Application.Common.Models;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// Use this instead of exceptions for expected failure paths.
///
/// Usage:
///   return Result.Success(value);
///   return Result.Failure&lt;T&gt;("Something went wrong.");
/// </summary>
public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error!);
}

/// <summary>Non-generic Result for operations that do not return a value.</summary>
public sealed class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result Failure(string error) => new(false, error);
    public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);

    /// <summary>Matches success or failure and produces a result of <typeparamref name="TResult"/>.</summary>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<string, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error!);
}
