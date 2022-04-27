using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GithubActions.Contract
{
    public class ApiBadRequestResponse
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ApiBadRequestResponse(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Message = "Invalid ModelState";
            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }

        public ApiBadRequestResponse(string message, IEnumerable<string> errors = null)
        {
            Message = message;

            errors ??= new List<string>();
            Errors = errors;
        }

        public ApiBadRequestResponse()
        {
        }
    }
}