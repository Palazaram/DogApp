using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DogApp.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    { 
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<ValidationError>();

                foreach (var key in context.ModelState.Keys)
                {
                    var errorMessages = context.ModelState[key].Errors
                        .Select(error => error.ErrorMessage)
                        .ToList();

                    errors.Add(new ValidationError
                    {
                        Field = key,
                        Messages = errorMessages
                    });
                }

                var result = new ObjectResult(errors)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };

                context.Result = result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public List<string> Messages { get; set; }
    }
}
