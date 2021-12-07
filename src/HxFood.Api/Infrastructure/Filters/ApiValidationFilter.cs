using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HxFood.Api.Infrastructure.Filters
{
    public class ApiValidationFilter : ActionFilterAttribute
    {
        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var data = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors.Select(b => b.ErrorMessage))
                    .ToList();
                    
                context.Result = new JsonResult(data) { StatusCode = 400 };
            }
        }
    }
}