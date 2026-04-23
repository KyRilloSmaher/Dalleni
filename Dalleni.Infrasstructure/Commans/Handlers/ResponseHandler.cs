using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.ResponsePattern;
using System.Net;

namespace Dalleni.Infrasstructure.Handlers
{
    public class ResponseHandler : IResponseHandler
    {
        public Response<T> Success<T>(T data, string message = "Success")
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message
            };
        }

        public Response<T> Created<T>(T data, string message = "Created successfully")
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = message
            };
        }

        public Response<T> BadRequest<T>(string message = "Bad request")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message
            };
        }

        public Response<T> Unauthorized<T>(string message = "Unauthorized")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message
            };
        }

        public Response<T> NotFound<T>(string message = "Not found")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message
            };
        }

        public Response<T> UnprocessableEntity<T>(string message = "Validation failed")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message
            };
        }

        public Response<T> ServerError<T>(string message = "Internal server error")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Succeeded = false,
                Message = message
            };
        }
    }
}