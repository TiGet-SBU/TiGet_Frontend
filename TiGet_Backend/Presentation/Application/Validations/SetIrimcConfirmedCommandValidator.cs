using System;
using System.Threading.Tasks;
using FluentValidation;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class SetIrimcConfirmedCommandValidator : AbstractValidator<SetIrimcConfirmedCommand>
    {
        private readonly IUserRepository _userRepository;
        public SetIrimcConfirmedCommandValidator(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.UserId).NotEmpty().WithMessage("USER_ID_IS_EMPTY");
        }
    }
}
