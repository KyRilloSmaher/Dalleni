using Dalleni.Domin.ResponsePattern;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Dalleni.Domin.Interfaces.Handlers
{
    public interface IResponseHandler
    {
         Response<T> Success<T>(T data, string message = "Success");
         Response<T> Created<T>(T data, string message = "Created successfully");
         Response<T> BadRequest<T>(string message = "Bad request");
         Response<T> Unauthorized<T>(string message = "Unauthorized");
         Response<T> NotFound<T>(string message = "Not found");
         Response<T> UnprocessableEntity<T>(string message = "Validation failed");
         Response<T> ServerError<T>(string message = "Internal server error");
    }
}
