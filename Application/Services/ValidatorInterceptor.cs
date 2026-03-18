using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {

            if (!result.IsValid)
            {
                var errors = result.Errors.GroupBy(m => m.PropertyName)
                   .ToDictionary(m => m.Key, v => v.Select(m => m.ErrorCode));


                throw new BadRequestException("Gonderilen melumatlar serti odemir", errors);
            }

            return result;
        }
    }
}
