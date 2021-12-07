using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HxFood.Api.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage).ToList();
        }
    }
}