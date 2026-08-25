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
}

