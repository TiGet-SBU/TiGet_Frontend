using FluentValidation;
using Rhazes.Services.Identity.API.Application.Commands;

namespace Identity.API.Application.Validations
{
    public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
    {

        //_userRepository = MethodParameterChecker.CheckUp(userRepository);
        public SetPasswordCommandValidator(
            //IUserRepository userRepository
            )
        {
            //private readonly IUserRepository _userRepository;
            RuleFor(command => command.Id).NotEmpty().WithMessage("USER_ID_IS_REQUIRED");
            RuleFor(command => command.CurrentPassword).NotEmpty().WithMessage("CURRENT_PASSWORD_IS_REQUIRED");
            RuleFor(command => new { command.Password, command.ConfirmPassword })
                .NotEmpty().WithMessage("PASSWORD_IS_EMPTY")
                .Must(x => CheckPassword(x.Password, x.ConfirmPassword)).WithMessage("THE_PASSWORD_IS_NOT_MATCH");
            RuleFor(command => new { command.Password, command.CurrentPassword })
                .NotEmpty().WithMessage("PASSWORD_IS_EMPTY")
                .Must(x => CheckCurrentAndNewPassword(x.Password, x.CurrentPassword)).WithMessage("THE_NEW_PASSWORD_MUST_BE_DIFFERENT_FROM_CURRENT_PASSWORD");
        }

        private bool CheckPassword(string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return true;
            else
                return false;
        }

        private bool CheckCurrentAndNewPassword(string password, string currentPassword)
        {
            if (password == currentPassword)
                return false;
            return true;
        }
    }

}
