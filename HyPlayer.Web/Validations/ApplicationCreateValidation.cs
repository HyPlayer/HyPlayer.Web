using FluentValidation;
using HyPlayer.Web.Infrastructure.Models.Packages.RequestPackages;

namespace HyPlayer.Web.Validations;

public class ApplicationCreateValidation : AbstractValidator<ApplicationCreateRequest>
{
    public ApplicationCreateValidation()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.ChannelType).IsInEnum();
        RuleFor(x => x.Contact).EmailAddress();
    }
}