using FluentValidation;
using HyPlayer.Web.Models.Packages;

namespace HyPlayer.Web.Validations;

public class ApplicationCreateValidation : AbstractValidator<UserCreateRequest>
{
    public ApplicationCreateValidation()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.ChannelType).IsInEnum();
        RuleFor(x => x.Contact).EmailAddress();
    }
}