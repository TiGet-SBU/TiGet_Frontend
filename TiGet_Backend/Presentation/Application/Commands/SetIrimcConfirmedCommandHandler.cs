using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class SetIrimcConfirmedCommandHandler : IRequestHandler<SetIrimcConfirmedCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public SetIrimcConfirmedCommandHandler(
            IMediator mediator,
            IUserRepository userRepository
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<bool> Handle(SetIrimcConfirmedCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user.UserType != UserType.Healer.Id && user.UserType != UserType.Healer_SystemManager.Id)
                return false;

            user.IrimcConfirmed = command.IrimcConfirmed;
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return command.IrimcConfirmed;
        }
    }
}
