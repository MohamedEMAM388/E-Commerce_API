using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API.Factories
{
    public static class ApiResponseFactory
    {

        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext) {

            var errors = actionContext.ModelState.Where(x => x.Value!.Errors.Count > 0)
                                .ToDictionary(
                                       x => x.Key, x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

            var problem = new ProblemDetails()
            {

                Title = "Validation Error",
                Detail = "One or more validation errors occurred",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { { "errors", errors } }

            };

            return new BadRequestObjectResult(problem);

        }
    }
}
