using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class VerifyTwoFactorTokenCommandValidator : AbstractValidator<VerifyTwoFactorTokenCommand>
    {
        IUserRepository _userRepository;
        public VerifyTwoFactorTokenCommandValidator(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.UserId).NotEmpty().WithMessage("USER_ID_IS_EMPTY")
                .Must(x => CheckUserId(x)).WithMessage("USER_ID_IS_INVALID");
            RuleFor(command => command.Token).NotEmpty().WithMessage("TOKEN_IS_EMPTY");
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
    }
}
