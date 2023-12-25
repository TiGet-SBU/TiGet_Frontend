using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CompleteRegisterCommandHandler : IRequestHandler<CompleteRegisterCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        public CompleteRegisterCommandHandler(
            IIdentityService identityService,
            IUserRepository userRepository
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<bool> Handle(CompleteRegisterCommand command, CancellationToken cancellationToken)
        {
            var userId =_identityService.GetUserId().Value;
            await _userRepository.CompleteRegister(userId);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return true;
        }
    }
}
