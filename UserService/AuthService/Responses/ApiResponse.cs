﻿namespace UserAPI.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }

    public static ApiResponse<T> SuccessResponse(T? data) =>
        new ApiResponse<T> { Success = true, Data = data };

    public static ApiResponse<T> ErrorResponse(string code, string message) =>
        new ApiResponse<T> { Success = false, Error = new ApiError { Code = code, Message = message } };
}

public class ApiError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
