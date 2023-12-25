using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class DeleteLogicalUserCommandHandler : IRequestHandler<DeleteLogicalUserCommand, UserDeleteLogicalDTO>
    {
        private readonly IUserRepository _userRepository;

        public DeleteLogicalUserCommandHandler(IUserRepository userRepository,IIdentityService identityService, IMediator mediator)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<UserDeleteLogicalDTO> Handle(DeleteLogicalUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var applicationUser = await _userRepository.GetByIdAsync(Guid.Parse(command.Id));

                var response = await _userRepository.DeleteLogicalAsync(applicationUser);

                await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return await Task.FromResult(UserDeleteLogicalDTO.FromApplicationUser(response));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class UserDeleteLogicalDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static UserDeleteLogicalDTO FromApplicationUser(ApplicationUser applicationUser)
        {
            return new UserDeleteLogicalDTO()
            {
                Id = applicationUser.Id.ToString(),
                LastName = applicationUser.LastName,
                Name = applicationUser.Name,
                Email = applicationUser.Email,
                NationalCode = applicationUser.UserName,
                UserName = applicationUser.UserName,
                PhoneNumber = applicationUser.PhoneNumber
            };
        }

    }
}