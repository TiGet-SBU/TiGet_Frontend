using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using System;
using System.Linq;

namespace Rhazes.Services.Identity.API.Infrastructure.Mapper
{
    public class DeviceInfoMapper : BaseMapper<DeviceInfo, DeviceInfoDTO>
    {
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;

        public DeviceInfoMapper(
             IIdentityService identityService,
             IPermissionService permissionService
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
        }

        public override DeviceInfoDTO ToDto(DeviceInfo entity)
        {

            return new DeviceInfoDTO()
            {
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                DeviceName = entity.DeviceName,
                AppVersion = entity.AppVersion,
                OS = entity.OS,
                OSVersion = entity.OSVersion,
                LastActivity = entity.LastActivity,
                GoogleToken = entity.GoogleToken
            };
        }

        public override DeviceInfo ToEntity(DeviceInfoDTO dto)
        {

            return new DeviceInfo()
            {
                UserId = _identityService.GetUserId().Value,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                DeviceName = dto.DeviceName,
                AppVersion = dto.AppVersion,
                OS = dto.OS,
                OSVersion = dto.OSVersion,
                LastActivity = dto.LastActivity,
                GoogleToken = dto.GoogleToken
            };

        }
    }
}
