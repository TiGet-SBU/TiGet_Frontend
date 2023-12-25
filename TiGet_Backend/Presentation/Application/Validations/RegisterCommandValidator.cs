using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Identity.API.Application.Commands;
using Identity.Domain.AggregatesModel.UserAggregate;
using System.Linq;
using System;

namespace Identity.API.Application.Validations
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IUserRepository _userRepository;
        public RegisterCommandValidator(
             IUserRepository userRepository,
            ILogger<RegisterCommand> logger
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.ObjectDTO.Name).NotEmpty().WithMessage("NAME_IS_REQUIRED");
            RuleFor(command => command.ObjectDTO.LastName).NotEmpty().WithMessage("LASTNAME_IS_REQUIRED");
            RuleFor(command => command.ObjectDTO.UserType).NotEmpty().WithMessage("TYPE_IS_REQUIRED")
                .Must(x => CheckUserType(x)).WithMessage("TYPE_IS_INVALID");

            RuleFor(command => new { command.ObjectDTO.Password, command.ObjectDTO.ConfirmPassword })
                .NotEmpty().WithMessage("PASSWORD_IS_EMPTY")
                .Must(x => CheckPassword(x.Password, x.ConfirmPassword)).WithMessage("THE_PASSWORD_IS_NOT_MATCH");

            RuleFor(command => new { command.ObjectDTO.PhoneNumber, command.ObjectDTO.UserId }).Cascade(CascadeMode.Stop)
              .Must(x => CheckPhoneNumberEmpty(x.PhoneNumber)).WithMessage("PHONE_NUMBER_IS_EMPTY")
              .Must(x => CheckPhoneNumberLength(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_LENGTH_IS_NOT_VALID")
              .Must(x => CheckPhoneNumberFormat(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_DOES_NOT_START_WITH_09")
                .Must(x => CheckDuplicatePhoneNumberAsync(x.PhoneNumber, x.UserId)).WithMessage("PHONE_NUMBER_IS_DUPLICATE");

            RuleFor(command => command.ObjectDTO.UserName).NotEmpty().WithMessage("USERNAME_IS_REQUIRED")
                //.Must(x => ValidationHelper.IsValidNationalCode(x)).When(x => x.ObjectDTO.NationalCode.Length == 10).WithMessage("NATIONAL_CODE_IS_INVALID")
                ;
            RuleFor(command => command.ObjectDTO.MedicalLicenseNumber)
                .Must(x => CheckMedicalLicenseNumber(x))
                .When(x => (x.ObjectDTO.UserType == UserType.Healer_SystemManager.Id || x.ObjectDTO.UserType == UserType.Healer.Id))
                .WithMessage("MEDICAL_LICENSE_NUMBER_IS_REQUIRED");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }

        private bool CheckPassword(string password, string confirmPassword)
        {
            if (password == confirmPassword)
                return true;
            else
                return false;
        }

        private bool CheckUserType(int userType)
        {
            if (!UserType.RegisterList().Any(A => A.Id == userType))
            {
                return false;
            }
            return true;
        }

        private bool CheckMedicalLicenseNumber(string medicalLicenseNumber)
        {
            if (!string.IsNullOrEmpty(medicalLicenseNumber))
            {
                return true;
            }
            return false;
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
