using Microsoft.Extensions.Logging;
using FluentValidation;
using Identity.API.Application.Commands;
using Identity.Domain.AggregatesModel.UserAggregate;
using FluentValidation.Validators;
using System;

namespace Identity.API.Application.Validations
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        [Obsolete]
        public UpdateProfileCommandValidator(ILogger<UpdateProfileCommandValidator> logger, IUserRepository _userRepository)
        {
            RuleFor(command => new { command.ObjectDTO.UserType, command.ObjectDTO.MedicalLicenseNumber }).NotEmpty()
                .Must((x, cancellation) => CheckMedicalLicenseNumber(x.ObjectDTO.UserType, x.ObjectDTO.MedicalLicenseNumber)).WithMessage("MEDICAL_LICENSE_NUMBER_IS_EMPTY");


            RuleFor(command => command.ObjectDTO.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("EMAIL_IS_EMPTY").EmailAddress(mode: EmailValidationMode.Net4xRegex).WithMessage("EMAIL_IS_INVALID");


            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }


        private bool CheckMedicalLicenseNumber(int userType,string medicalLicenseNumber)
        {
            if ((userType == UserType.Healer.Id || userType == UserType.Healer_SystemManager.Id) && string.IsNullOrEmpty(medicalLicenseNumber))
            {
                return false;
            }
            return true;
          
        }

    }
}