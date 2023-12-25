using MediatR;
using Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Domain.Validators;
using Identity.API.Application.DTO;

namespace Identity.API.Application.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;
        private readonly IValidator<ApplicationUser> _userValidator;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IMediator mediator,
            IValidator<ApplicationUser> userValidator,
            IMapper<ApplicationUser, UserPatientDTO> mapper
            )
        {
            _userRepository =  MethodParameterChecker.CheckUp(userRepository); ;
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _userValidator = MethodParameterChecker.CheckUp(userValidator);
        }

        public async Task<UserPatientDTO> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var applicationUser = _mapper.ToEntity(command.ObjectDTO);
                await _userValidator.Validate(applicationUser);
                var response = await _userRepository.RegisterAsync(applicationUser, command.ObjectDTO.Password);
                return await Task.FromResult(_mapper.ToDto(response));
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

    }
}