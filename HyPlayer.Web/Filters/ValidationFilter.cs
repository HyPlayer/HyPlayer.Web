using FluentValidation;

namespace HyPlayer.Web.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(t => t?.GetType() == typeof(T)) is not T validatableObject)
            return Results.BadRequest();
        var validationResult = await validator.ValidateAsync(validatableObject);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(string.Join(';',validationResult.Errors.Select(t => t.ErrorMessage)));
        }

        return await next(context);
    }
}