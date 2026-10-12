using RaizesDoNordeste.Domain.Enums;

namespace RaizesDoNordeste.Application.Commons;

public class ResultViewModel
{
    public ResultViewModel(bool isSuccess = true, string message = "", ResultStatus status = ResultStatus.Success)
    {
        IsSuccess = isSuccess;
        Message = message;
        Status = status;
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public ResultStatus Status { get; private set; }

    public static ResultViewModel Success()
        => new();

    public static ResultViewModel Error(string message, ResultStatus status = ResultStatus.ValidationError)
        => new(false, message, status);
}

public class ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(T? data, bool isSuccess = true, string message = "", ResultStatus status = ResultStatus.Success)
        : base(isSuccess, message, status)
    {
        Data = data;
    }

    public T? Data { get; private set; }

    public static ResultViewModel<T> Success(T data, string message = "")
        => new(data, true, message);

    public static ResultViewModel<T> Error(string message, ResultStatus status = ResultStatus.ValidationError)
        => new(default, false, message, status);
}