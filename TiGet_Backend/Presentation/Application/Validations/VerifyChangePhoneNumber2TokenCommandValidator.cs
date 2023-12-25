using System;
using System.Linq;
using FluentValidation;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class VerifyChangePhoneNumber2TokenCommandValidator : AbstractValidator<VerifyChangePhoneNumber2TokenCommand>
    {
        private readonly IUserRepository _userRepository;
        public VerifyChangePhoneNumber2TokenCommandValidator(IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
            RuleFor(command => command.UserId).NotEmpty().WithMessage("USERID_IS_REQUIRED").Must(x => IsValidUser(x)).WithMessage("USER_IS_NOT_EXIST");
            RuleFor(command => command.Token).NotEmpty().WithMessage("TOKEN_IS_REQUIRED");
            RuleFor(command => command.PhoneNumber).NotEmpty().WithMessage("PHONE_NUMBER_IS_EMPTY")
                .Length(11).WithMessage("THE_PHONE_NUMBER_LENGTH_IS_NOT_VALID")
                .Must(x => CheckPhoneNumberFormat(x)).WithMessage("THE_PHONE_NUMBER_DOES_NOT_START_WITH_09");
        }

        private bool IsValidUser(Guid userId)
        {
            var result =  _userRepository.GetQueryable().Result;
            return result.Any(a => a.Id == userId);
        }

        private bool CheckPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            if (phoneNumber.StartsWith("09"))
                return true;
            return false;
        }

    }
}
