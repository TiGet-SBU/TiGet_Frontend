using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Domain.Validators;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.IntegrationEvents;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CreateOrUpdateUserPatientCommandHandler : IRequestHandler<CreateOrUpdateUserPatientCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;
        private readonly IValidator<ApplicationUser> _userValidator;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public CreateOrUpdateUserPatientCommandHandler(
            IUserRepository userRepository,
            IMediator mediator,
            IValidator<ApplicationUser> userValidator,
            IMapper<ApplicationUser, UserPatientDTO> mapper,
            IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _userValidator = MethodParameterChecker.CheckUp(userValidator);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
        }

        public async Task<UserPatientDTO> Handle(CreateOrUpdateUserPatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var applicationUser = await _userRepository.GetByNationalCode(command.ObjectDTO.NationalCode);
                if (applicationUser == null)
                {
                    command.ObjectDTO.UserType = UserType.Patient.Id;
                    applicationUser = _mapper.ToEntity(command.ObjectDTO);

                    var response = await _userRepository.RegisterAsync(applicationUser, command.ObjectDTO.Password);
                }
                else if(applicationUser.UserType == UserType.Patient.Id)
                {
                    command.ObjectDTO.Id = applicationUser.Id;
                    applicationUser = _mapper.ToEntity(command.ObjectDTO);

                    var response = await _userRepository.UpdateAsync(applicationUser);
                }

                return await Task.FromResult(_mapper.ToDto(applicationUser));
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}