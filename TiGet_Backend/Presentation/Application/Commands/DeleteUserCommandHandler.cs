using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDeleteDTO>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<UserDeleteDTO> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var applicationUser = await _userRepository.GetByIdAsync(Guid.Parse(command.ObjectDTO.Id));

                var response = await _userRepository.DeleteAsync(applicationUser);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);


                return await Task.FromResult(UserDeleteDTO.FromApplicationUser(response));
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }

}

