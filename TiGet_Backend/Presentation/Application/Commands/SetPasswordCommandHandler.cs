using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public SetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<bool> Handle(SetPasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _userRepository.ChangePasswordAsync(command.Id, command.CurrentPassword, command.Password);
                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }




}

