using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CreateOrUpdateDeviceInfoCommandHandler : IRequestHandler<CreateOrUpdateDeviceInfoCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<DeviceInfo, DeviceInfoDTO> _mapper;

        public CreateOrUpdateDeviceInfoCommandHandler(
            IUserRepository userRepository,
            IMapper<DeviceInfo, DeviceInfoDTO> mapper
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository); ;
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<bool> Handle(CreateOrUpdateDeviceInfoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userRepository.AddOrUpdateDeviceInfoAsync(_mapper.ToEntity(new DeviceInfoDTO()
                {
                    FullName = command.FullName,
                    PhoneNumber = command.PhoneNumber,
                    DeviceName = command.DeviceName,
                    OS = command.OS,
                    OSVersion = command.OSVersion,
                    AppVersion = command.AppVersion,
                    LastActivity = command.LastActivity,
                    GoogleToken = command.GoogleToken
                }));
                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}