using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class SetCurrentTenantUserCommandHandler : IRequestHandler<SetCurrentTenantUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _iIdentityService;

        public SetCurrentTenantUserCommandHandler(
            IUserRepository userRepository,
            IIdentityService iIdentityService
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _iIdentityService = MethodParameterChecker.CheckUp(iIdentityService);
        }

        public async Task<bool> Handle(SetCurrentTenantUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_iIdentityService.GetUserId().Value);

            user.CurrentTenantId = command.TenantId;
            user.UserType = command.UserTypeId;
            
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(true);
        }
    }
}