using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API.CustomMiddelWares
{
    public class ExceptionHandlerMiddelWare
    {
        // to act as a middleware the class must have 
        // 1 => must have a constructor take one paramter from type requestdelegate 
        // 2 => class must contain a function called invoke or invokeAsync take one paramter from type httpContext

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddelWare> _logger;

        public ExceptionHandlerMiddelWare(RequestDelegate next , 
            ILogger<ExceptionHandlerMiddelWare> logger) 
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext) {

            try
            {

                await _next.Invoke(httpContext);
                // handel not found end point
                if (httpContext.Response.StatusCode == 404
                    && httpContext.GetEndpoint() == null)
                {

                    var response = new ProblemDetails() { 
                    
                        Title = "Error While Processing HTTP Request End Point NotFound",
                        Status = StatusCodes.Status404NotFound,
                        Detail = $"The requested endpoint {httpContext.Request.Path} was not found.",
                        Instance = httpContext.Request.Path
                    };

                    await httpContext.Response.WriteAsJsonAsync(response);
                }

            }
            catch (Exception ex) // catch any unhandled exception that occurs during the request processing
            {

                // log error
                _logger.LogError(ex , "An error occurred while processing the request.");

                // return custom error response
                var response = new ProblemDetails() {
                    Title = "An Unexpected Error Occured",
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    },
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path
                };

                httpContext.Response.StatusCode = response.Status.Value;

                await httpContext.Response.WriteAsJsonAsync(response); 
            }

        }
    }
}
