using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.API.Application.IntegrationEvents;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.Services.Identity.API.Application.Queries;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            ILogger<RegisterCommandHandler> logger,
            IMapper<ApplicationUser, UserPatientDTO> mapper,
            IMediator mediator,
            IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
        }

        public async Task<UserPatientDTO> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var user = new UserPatientDTO();
            if (command.ObjectDTO.Id != null)
            {
                var applicationUser =await (await _userRepository.GetQueryable()).FirstOrDefaultAsync(x=>x.Id == command.ObjectDTO.Id);
                user = _mapper.ToDto(applicationUser);

                if (user.UserType == UserType.Patient.Id)
                {
                    UpdateUserCommand updateUserCommand = new(
                        command.ObjectDTO.Id.Value,
                        user.UserName,
                        user.Email,
                        command.ObjectDTO.PhoneNumber,
                        command.ObjectDTO.Name,
                        command.ObjectDTO.LastName,
                        command.ObjectDTO.Password,
                        command.ObjectDTO.ConfirmPassword,
                        command.ObjectDTO.UserType,
                        phoneNumberConfirmed: false,
                        irimcConfirmed: false,
                        finalConfirmed: false);
                    user = await _mediator.Send(updateUserCommand, cancellationToken);

                 
                }

                return user;
            }

            var createUserCommand = new CreateUserCommand(
                command.ObjectDTO.NationalCode,
                $"default@gmail.com",
                command.ObjectDTO.PhoneNumber,
                command.ObjectDTO.Password,
                command.ObjectDTO.ConfirmPassword,
                command.ObjectDTO.Name,
                command.ObjectDTO.LastName,
                command.ObjectDTO.NationalCode,
                command.ObjectDTO.UserType,
                command.ObjectDTO.Gender,
                command.ObjectDTO.MedicalLicenseNumber
                );

            user = await _mediator.Send(createUserCommand, cancellationToken);

            return user;
        }
    }
}