using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Framework.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static ValidationException ToVaildationException(this Exception exception)
        {
            var error = new ValidationFailure(string.Empty, exception.Message);
            return new ValidationException(new ValidationFailure[] { error });
        }

        public static ValidationException ValidationException(this IEnumerable<ValidationFailure> errors)
        {
            return new ValidationException(errors.Select(x => new ValidationFailure(x.ErrorCode, x.ErrorMessage)));
        }

        public static BaseResponseError<BaseResponseErrorItem> ToResponse(this ValidationException ex)
        {
            var response = ex.Errors.Select(x => new BaseResponseErrorItem(x.PropertyName, ResponseCode.Invalid, x.ErrorMessage));
            return new BaseResponseError<BaseResponseErrorItem>(response, ResponseCode.Invalid, ex.Message);
        }

        public static async Task TryRunAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (ValidationException vex)
            {
                Console.WriteLine(vex.Message);
            }
            catch (Exception ex)
            {
                ex.Message.ToConsoleAsError();
            }

        }

        public static async Task<T> TryRunAsync<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ex.Message.ToConsoleAsError();
            }
            return await Task.FromResult(default(T));
        }

        public static void ToConsoleAsError(this string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();

        }
    }

    #region Core validator interceptor
    public class CoreValidatorInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new ValidationException("Request content invalid", result.Errors);
            }
            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
    #endregion
}
