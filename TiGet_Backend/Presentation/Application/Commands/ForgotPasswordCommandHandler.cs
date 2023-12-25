using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IMediator mediator
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<bool> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByNationalCode(command.UserName);
            await _mediator.Send(new SendResetPasswordTokenCommand(user.Id, user.PhoneNumber), cancellationToken);

            return true;
        }
    }
}
