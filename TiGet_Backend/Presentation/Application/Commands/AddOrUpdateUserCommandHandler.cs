using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Identity.API.Application.DTO;
using Identity.Domain.AggregatesModel.UserAggregate;
using Identity.Domain.Validators;

namespace Identity.API.Application.Commands
{
    public class AddOrUpdateUserCommandHandler : IRequestHandler<AddOrUpdateUserCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;
        private readonly IValidator<ApplicationUser> _userValidator;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;


        public AddOrUpdateUserCommandHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper,
            IValidator<ApplicationUser> userValidator,
            IMediator mediator,
            IIdentityService identityService
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _userValidator = MethodParameterChecker.CheckUp(userValidator);
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityService = MethodParameterChecker.CheckUp(identityService);
        }

        public async Task<UserPatientDTO> Handle(AddOrUpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByNationalCode(command.ObjectDTO.UserName);
            var applicationUser = _mapper.ToEntity(command.ObjectDTO);

            if (user != null)
            {
                user.UserName = applicationUser.UserName;
                user.NormalizedUserName = applicationUser.UserName;
                user.Name = applicationUser.Name;
                user.LastName = applicationUser.LastName;
                user.UserType = applicationUser.UserType;
                user.ModifiedById = _identityService.GetUserId().Value;
                user.ModifiedDate = applicationUser.ModifiedDate;
                user.Deleted = applicationUser.Deleted;
                user.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                user.NormalizedEmail = applicationUser.NormalizedEmail;
                user.Email = applicationUser.Email;
                user.MedicalLicenseNumber = applicationUser.MedicalLicenseNumber;
                user.IrimcConfirmed = true;

                if (!user.PhoneNumberConfirmed)
                {
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                }
            }
            else
            {
                await _userValidator.Validate(applicationUser);
                user = await _userRepository.AddAsync(applicationUser);
            }

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(_mapper.ToDto(user));
        }
    }
}