using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
        }

        public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            var applicationUser = await _roleRepository.GetByIdAsync(command.Id);

            var response = await _roleRepository.DeleteAsync(applicationUser);

            await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(true);
        }

    }


}

