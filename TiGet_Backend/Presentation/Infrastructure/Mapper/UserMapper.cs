using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;

namespace Rhazes.Services.Identity.API.Infrastructure.Mapper
{
    public class UserMapper : BaseMapper<ApplicationUser, UserPatientDTO>
    {
        private readonly IIdentityService _identityService;

        public UserMapper(
             IIdentityService identityService
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
        }

        public override UserPatientDTO ToDto(ApplicationUser entity)
        {
            return new UserPatientDTO()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Name = entity.Name,
                LastName = entity.LastName,
                UserType = entity.UserType,
                Gender = entity.Gender,
                UserTypeName = UserType.From(entity.UserType).Name,
                NationalCode = entity.UserName,
                MedicalLicenseNumber = entity.MedicalLicenseNumber,
                Email = entity.Email,
                RegisterCompleted = entity.RegisterCompleted,
                PhoneNumber = entity.PhoneNumber,
                PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                PhoneNumber2 = entity.PhoneNumber2,
                PhoneNumber2Confirmed = entity.PhoneNumber2Confirmed,
                IrimcConfirmed = entity.IrimcConfirmed,
                FinalConfirmed = entity.FinalConfirmed,
                Deleted = entity.Deleted,
                CreateDate = entity.CreateDate,    
                CurrentTenantId = entity.CurrentTenantId
            };
        }

        public override ApplicationUser ToEntity(UserPatientDTO dto)
        {
            var now = DateTime.Now;
            ApplicationUser res;
            var id = (Guid.Empty.Equals(dto.Id) || dto.Id == null) ? Guid.NewGuid() : dto.Id.Value;
            var currentUserId = _identityService.GetUserId() == null ?  id : _identityService.GetUserId().Value;


            res = new ApplicationUser()
            {
                Id = id,
                UserName = dto.UserName,
                Name = dto.Name,
                LastName = dto.LastName,
                UserType = dto.UserType,
                Gender = dto.Gender,
                Deleted = false,
                PhoneNumber = dto.PhoneNumber,
                MedicalLicenseNumber = dto.MedicalLicenseNumber,
                TwoFactorEnabled = false,
                NormalizedEmail = dto.Email.ToUpper(),
                NormalizedUserName = dto.UserName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Email = dto.Email,
                EmailConfirmed = true,
                CreateById = currentUserId,
                CreateDate = now,
                ModifiedById = currentUserId,
                ModifiedDate = now,
                PasswordHash = dto.Password
            };

            return res;
        }
    }
}
