using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.CommonResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseController : ControllerBase
    {

        // we have two standard response 
        // 1 => one without value 

        protected IActionResult HandleResult(Result result) { 
            
            // if success 
            if(result.IsSuccess)
                return NoContent();
            else // if failuer
                return HandleProblem(result.Errors);

        }

        // 2 => another one with value 

        protected ActionResult HandleResult<TValue>(Result<TValue> result) {

            // if success
            if (result.IsSuccess)
                return Ok(result.Value);
            else // if failuer
                return HandleProblem(result.Errors);

        }


        // handle error response
        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no error provided, return 500 internal server error
            if (errors.Count == 0)
                return Problem(
                    title: "An Error Occurred",
                    statusCode: StatusCodes.Status500InternalServerError
                );

            // if all errors are validation error handle them as a validation error
            if(errors.All(e => e.ErrorType == ErrorType.Validation))
                return HandleValidationErrors(errors);
    

            // if has one error , hadle it as a single error
            return HandleSingleError(errors[0]);
        }

        // handle single error response
        private ActionResult HandleSingleError(Error error) {

            return Problem(
                
                title: error.Code,
                detail: error.Description,
                type : error.ErrorType.ToString(),
                statusCode : MapErrorTypeIntoStatusCode(error.ErrorType)
            );
        }

        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors) {

            var modelState = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelState);

        }


        private static int MapErrorTypeIntoStatusCode(ErrorType type)
                           //  applay startegy pattern to map error type to status code
                           => type switch {

                               ErrorType.NotFound => StatusCodes.Status404NotFound,
                               ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                               ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                               ErrorType.InvalidCredential => StatusCodes.Status401Unauthorized,
                               ErrorType.Validation => StatusCodes.Status400BadRequest,
                               _=> StatusCodes.Status500InternalServerError
                           };
    }
}
