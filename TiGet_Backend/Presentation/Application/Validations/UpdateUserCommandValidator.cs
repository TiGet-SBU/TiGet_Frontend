using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Linq;
using System;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandValidator(ILogger<UpdateUserCommandValidator> logger, IUserRepository userRepository)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);

            RuleFor(command => command.ObjectDTO.Name).NotEmpty().WithMessage("NAME_IS_REQUIRED"); ;
            RuleFor(command => command.ObjectDTO.LastName).NotEmpty().WithMessage("LASTNAME_IS_REQUIRED");
            RuleFor(command => command.ObjectDTO.UserName).NotEmpty().WithMessage("USERNAME_IS_REQUIRED");
            //.Must(x => ValidationHelper.IsValidNationalCode(x)).When(x => x.ObjectDTO.UserName.Length == 10).WithMessage("NATIONAL_CODE_IS_INVALID")

            //RuleFor(command => command.ObjectDTO.NationalCode).NotEmpty().WithMessage("NATIONAL_CODE_IS_REQUIRED");
            RuleFor(command => new { command.ObjectDTO.Password, command.ObjectDTO.ConfirmPassword }).NotEmpty()
                .Must(x => CheckPassword(x.Password, x.ConfirmPassword)).WithMessage("THE_PASSWORD_IS_NOT_MATCH");
            RuleFor(command => new { command.ObjectDTO.Id, command.ObjectDTO.UserName }).NotEmpty().WithMessage("THE_USER_NAME_IS_EXIST");

            RuleFor(command => new { command.ObjectDTO.PhoneNumber, command.ObjectDTO.UserId }).Cascade(CascadeMode.Stop)
           .Must(x => CheckPhoneNumberEmpty(x.PhoneNumber)).WithMessage("PHONE_NUMBER_IS_EMPTY")
           .Must(x => CheckPhoneNumberLength(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_LENGTH_IS_NOT_VALID")
           .Must(x => CheckPhoneNumberFormat(x.PhoneNumber)).WithMessage("THE_PHONE_NUMBER_DOES_NOT_START_WITH_09")
            .Must(x => CheckDuplicatePhoneNumberAsync(x.PhoneNumber, x.UserId)).WithMessage("PHONE_NUMBER_IS_DUPLICATE");

            RuleFor(command => command.ObjectDTO.UserType).NotEmpty().WithMessage("TYPE_IS_REQUIRED")
                .Must(x => CheckUserType(x)).WithMessage("TYPE_IS_INVALID");

            RuleFor(command => command.ObjectDTO.Email).NotEmpty().WithMessage("EMAIL_IS_EMPTY");

            // TODO: move below rules to staff DTO validator in Human.API
            //RuleFor(command => command.ObjectDTO.StateId).NotEmpty().WithMessage("SELECT_STATE_IS_REQUIRED");
            //RuleFor(command => command.ObjectDTO.NationalityId).NotEmpty().WithMessage("SELECT_NATIONALITY_IS_REQUIRED");
            //RuleFor(command => command.ObjectDTO.Tel).MustAsync((x, cancellation) => CheckPhoneNumber(x)).WithMessage("THE_TEL_NUMBER_IS_NOT_VALID");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }

        //private bool BeValidExpirationDate(DateTime dateTime)
        //{
        //    return dateTime >= DateTime.UtcNow;
        //}

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