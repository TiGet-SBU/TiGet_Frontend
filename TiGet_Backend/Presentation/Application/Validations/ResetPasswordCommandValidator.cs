using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        IUserRepository _userRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        public ResetPasswordCommandValidator(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.UserId).NotEmpty().WithMessage("USER_ID_IS_EMPTY")
                .Must(x  => CheckUserId(x)).WithMessage("USER_ID_IS_INVALID");
            RuleFor(command => command.Token).NotEmpty().WithMessage("TOKEN_IS_EMPTY");
            RuleFor(command => command.Password).NotEmpty().WithMessage("PASSWORD_IS_EMPTY");
            RuleFor(command => new { command.Password, command.UserId }).Must(x => CheckEqualOldPassword(x.Password, x.UserId));
        }

        private bool CheckUserId(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                var result = _userRepository.GetQueryable().Result;
                return result.Any(a => a.Id == userId);
            }
            return true;
        }

        private bool CheckEqualOldPassword(string password, Guid userId)
        {
            if (password == null)
            {
                var user = _userRepository.GetByIdAsync(userId).Result;
                var hashPassword = _passwordHasher.HashPassword(user, password);
                var result = _userRepository.GetQueryable().Result;
                return result.Any(a => a.PasswordHash == hashPassword);
            }
            return true;
        }
    }
}
