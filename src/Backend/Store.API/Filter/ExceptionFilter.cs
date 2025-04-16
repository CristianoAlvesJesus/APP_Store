using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Communication.Responses;
using Store.Exceptions;
using Store.Exceptions.ExceptionBase;
using System.Net;

namespace Store.API.Filter;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is StoreException storeException)
            HandleProjecException(storeException, context);
        else
            ThrowUnknowException(context);
    }

    private static void HandleProjecException(StoreException storeException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)storeException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(storeException.GetErrorMessages())); 
    }

    private static void ThrowUnknowException(ExceptionContext contex)
    {
        contex.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        contex.Result = new ObjectResult(new ResponseErrorJson(ResourceMessageException.UNKNOWN_ERROR));
    }
}