namespace ECommerence_CleanArch.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string? ErorrMessage { get; }

    public T? Data { get; }

    private Result(bool ısSuccess,T? data ,string? erorrMessage)
    {
        IsSuccess = ısSuccess;
        Data = data;
        ErorrMessage = erorrMessage;
    }

    public static Result<T> Success(T data) => new(true,data,null);

    public static Result<T> Failure(string error) => new(false,default, error);

}
public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    private Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}
