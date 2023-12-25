using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Domain.Validators;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;
        private readonly IValidator<ApplicationUser> _userValidator;
        private readonly IMediator _mediator;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper,
            IValidator<ApplicationUser> userValidator,
            IMediator mediator
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _userValidator = MethodParameterChecker.CheckUp(userValidator);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<UserPatientDTO> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var applicationUser = _mapper.ToEntity(command.ObjectDTO);
            await _userValidator.Validate(applicationUser);
            
            var response = await _userRepository.UpdateAsync(applicationUser);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(_mapper.ToDto(response));
        }
    }
}