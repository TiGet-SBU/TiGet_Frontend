using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Exception;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper<ApplicationUserRole, UserRoleDTO> _mapper;

        public CreateUserRoleCommandHandler(IUserRepository userRepository, IIdentityService identityService, IMapper<ApplicationUserRole, UserRoleDTO> mapper)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<bool> Handle(CreateUserRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(command.UserId);
                var currentUser = await _userRepository.GetByIdAsync(_identityService.GetUserId().Value);
                if (user == null)
                    throw new DomainException("USER_INVALID");


                var response = await _userRepository.AddUserRolesAsync(user, command.Roles, currentUser.CurrentTenantId);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);


                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }


}

