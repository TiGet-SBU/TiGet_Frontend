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

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CreateUserRoleSystemicCommandHandler : IRequestHandler<CreateUserRoleSystemicCommand, bool>
    {
        private readonly IUserRepository _userRepository;
       
        private readonly IMapper<ApplicationUserRole, UserRoleDTO> _mapper;

        public CreateUserRoleSystemicCommandHandler(IUserRepository userRepository, IMapper<ApplicationUserRole, UserRoleDTO> mapper)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<bool> Handle(CreateUserRoleSystemicCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(command.UserId);
                
                if (user == null)
                    throw new DomainException("USER_INVALID");


                var response = await _userRepository.AddUserRolesSystemicAsync(user, command.Roles, command.TenantIds);

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

