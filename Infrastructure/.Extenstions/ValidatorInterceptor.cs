using Application.Common.Abstractions;
using Application.Common.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        private readonly ILocalizeServices _ls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ls"></param>
        public ValidatorInterceptor(ILocalizeServices ls)
        {
            _ls = ls;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="validationContext"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new ValidationException(_ls.Get(Modules.Core, "Message", MessageKey.E_005), result.Errors);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="commonContext"></param>
        /// <returns></returns>
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
