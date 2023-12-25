using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateOrUpdateUserPatientCommand : BaseCommand<UserPatientDTO>
    {

        public CreateOrUpdateUserPatientCommand(
        string nationalCode,
        string name,
        string lastName,
        string phoneNumber,
        int gender,
        DateTime? birthdate,
        Guid createById,
        Guid nationalityId,
        string fatherName,
        int? maritalType,
        Guid? birthStateId,
        Guid? birthCityId,
        string tel,
        Guid? stateId,
        Guid? cityId,
        string address,
        string zipCode,
        int? bloodTypeId,
        int? spouseBloodTypeId,
        Guid? educationId,
        Guid? jobId
            )
        {
            ObjectDTO.UserName = nationalCode;
            ObjectDTO.NationalCode = nationalCode;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.PhoneNumber = phoneNumber;
            ObjectDTO.Password = nationalCode;
            ObjectDTO.ConfirmPassword = nationalCode;
            ObjectDTO.UserType = UserType.Patient.Id;
            ObjectDTO.Email = $"default@gmail.com";
            ObjectDTO.Gender = gender;
            ObjectDTO.NationalityId = nationalityId;
            ObjectDTO.Birthdate = birthdate;
            ObjectDTO.CreateById = createById;
            ObjectDTO.NationalityId = nationalityId;

            ObjectDTO.FatherName = fatherName;
            ObjectDTO.MaritalType = maritalType;
            ObjectDTO.BloodTypeId = bloodTypeId;
            ObjectDTO.SpouseBloodTypeId = spouseBloodTypeId;
            ObjectDTO.BirthStateId = birthStateId;
            ObjectDTO.BirthCityId = birthCityId;
            ObjectDTO.Tel = tel;
            ObjectDTO.StateId = stateId;
            ObjectDTO.CityId = cityId;
            ObjectDTO.Address = address;
            ObjectDTO.ZipCode = zipCode;
            ObjectDTO.EducationId = educationId;
            ObjectDTO.JobId = jobId;
        }
    }
}
