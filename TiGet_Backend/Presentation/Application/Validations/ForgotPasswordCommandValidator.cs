using FluentValidation;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Utility;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        public ForgotPasswordCommandValidator(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            RuleFor(command => command.UserName).NotEmpty().WithMessage("USERNAME_IS_REQUIRED")
                .Must(x => ValidationHelper.IsValidNationalCode(x)).WithMessage("USERNAME_FORMAT_IS_INVALID")
                .MustAsync((x, cancellation) => CheckUserNameExist(x)).WithMessage("USERNAME_DOES_NOT_EXIST")
                .MustAsync((x, cancellation) => CheckPhoneNumberConfirmed(x)).WithMessage("PHONE_NUMBER_NOT_CONFIRMED")
                ;
        }

        public async Task<bool> CheckUserNameExist(string userName)
        {
            if (userName != null)
            {
                var result = await _userRepository.GetQueryable();
                return result.Any(a => a.UserName == userName);
            }
            return true;
        }

        public async Task<bool> CheckPhoneNumberConfirmed(string userName)
        {
            if (userName != null)
            {
                var result = await _userRepository.GetQueryable();
                return result.Any(a => a.UserName == userName && a.PhoneNumberConfirmed);
            }
            return true;
        }
    }
}
