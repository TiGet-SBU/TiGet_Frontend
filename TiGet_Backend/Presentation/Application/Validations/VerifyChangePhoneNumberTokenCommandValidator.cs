using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class VerifyChangePhoneNumberTokenCommandValidator : AbstractValidator<VerifyChangePhoneNumberTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        public VerifyChangePhoneNumberTokenCommandValidator(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.UserId).NotEmpty().WithMessage("USERID_IS_REQUIRED").Must(x => IsValidUser(x)).WithMessage("USER_IS_NOT_EXIST");
            RuleFor(command => command.Token).NotEmpty().WithMessage("TOKEN_IS_REQUIRED");
            RuleFor(command => new { command.PhoneNumber, command.UserId }).Cascade(CascadeMode.Stop)
                .Must(x => CheckPhoneNumberEmpty(x.PhoneNumber)).WithMessage("PHONE_NUMBER_IS_EMPTY")
                .Must(x => CheckPhoneNumberLength(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_LENGTH_IS_NOT_VALID")
                .Must(x => CheckPhoneNumberFormat(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_DOES_NOT_START_WITH_09")
                  .Must(x => CheckDuplicatePhoneNumberAsync(x.PhoneNumber, x.UserId)).WithMessage("PHONE_NUMBER_IS_DUPLICATE");
        }

        private bool IsValidUser(Guid userId)
        {
            var result = _userRepository.GetQueryable().Result;
            return result.Any(a => a.Id == userId);
        }


        private bool CheckPhoneNumberEmpty(string phoneNumber)
        {

            if (!string.IsNullOrEmpty(phoneNumber))
                return true;
            return false;
        }

        private bool CheckPhoneNumberLength(string phoneNumber)
        {

            if (phoneNumber.Length == 11)
                return true;
            return false;
        }

        private bool CheckPhoneNumberFormat(string phoneNumber)
        {

            if (phoneNumber.StartsWith("09"))
                return true;
            return false;
        }

        private bool CheckDuplicatePhoneNumberAsync(string phoneNumber, Guid id)
        {
            if ((_userRepository.GetQueryable()).Result.Any(x => x.PhoneNumber == phoneNumber && x.Id != id && x.UserType != UserType.Patient.Id))
            {
                return false;
            }
            return true;
        }
    }
}
