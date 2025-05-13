namespace GameService.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public int? StatusCode { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, T? value, string? error, int? statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value) => new(true, value, null, null);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, statusCode);
}
