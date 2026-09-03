using Dalleni.Domin.ResponsePattern;
using System.Net;

namespace Dalleni.UnitTests.Shared.Responses;

public static class ResponseFactory
{
    public static Response<T> Ok<T>(T data, string message = "Success")
    {
        return new Response<T>
        {
            Data = data,
            Message = message,
            StatusCode = HttpStatusCode.OK,
            Succeeded = true
        };
    }

    public static Response<T> BadRequest<T>(string message = "Bad request")
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false
        };
    }

    public static Response<T> Unauthorized<T>(string message = "Unauthorized")
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = false
        };
    }

    public static Response<T> NotFound<T>(string message = "Not found")
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false
        };
    }

    public static Response<T> Forbidden<T>(string message = "Forbidden")
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = false
        };
    }

    public static Response<T> Conflict<T>(string message = "Conflict")
    {
        return new Response<T>
        {
            Message = message,
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false
        };
    }
}
