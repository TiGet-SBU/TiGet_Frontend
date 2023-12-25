using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Infrastructure.Mapper
{
    public class RegisterMapper: BaseMapper<ApplicationUser, UserDTO>
    {
        private readonly IIdentityService _identityService;

        public RegisterMapper(
             IIdentityService identityService
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
        }

        public override UserDTO ToDto(ApplicationUser entity)
        {
            return new UserDTO()
            {
               Id  = entity.Id,
               UserName = entity.UserName,
               Name = entity.Name,
               LastName = entity.LastName,
               Type = entity.Type,
               TypeName = UserType.From(entity.Type).Name,
               PhoneNumber = entity.PhoneNumber,
               NationalCode = entity.UserName,
               Email = entity.Email,
               IrimcConfirmed = entity.IrimcConfirmed,
               RegisterCompleted = entity.RegisterCompleted,
               PhoneNumberConfirmed = entity.PhoneNumberConfirmed
            };
        }

        public override ApplicationUser ToEntity(UserDTO dto)
        {
            var now = DateTime.Now;
            var id = Guid.NewGuid();
            var res = new ApplicationUser()
            {
                Id = id,
                UserName = dto.UserName,
                Name = dto.Name,
                LastName = dto.LastName,
                Type = dto.Type,
                ModifiedById = id,
                ModifiedDate = now,
                Deleted = false,
                PhoneNumber = dto.PhoneNumber,
                TwoFactorEnabled = false,
                NormalizedEmail = dto.Email.ToUpper(),
                NormalizedUserName = dto.UserName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CreateById = id,
                CreateDate = now
            };

            return res;
        }
    }
}
