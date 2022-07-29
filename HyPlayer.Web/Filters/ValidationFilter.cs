using FluentValidation;
using HyPlayer.Web.Infrastructure.Models.Packages.ResponsePackages;

namespace HyPlayer.Web.Filters;

public class ValidationFilter<T> : IRouteHandlerFilter where T: class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(t => t?.GetType() == typeof(T)) is not T validatableObject)
            return Results.BadRequest();
        var validationResult = await _validator.ValidateAsync(validatableObject);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(new ValidationFailedResponse()
            {
                Errors = validationResult.Errors.Select(t=>t.ErrorMessage)
            });
        }

        return await next(context);
    }
}