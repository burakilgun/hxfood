using System.Collections.Generic;
using System.Linq;

namespace HxFood.Api.Models.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            Errors = new List<string>();
        }

        public BaseResponse(T entity)
        {
            Data = entity;
        }
        
        public T Data { get; set; }

        public bool HasError => Errors.Any();

        public List<string> Errors { get; set; }

        public void AddError(string message)
        {
            Errors.Add(message);
        }
    }
}