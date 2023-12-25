using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Domain.Validators;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;
        private readonly IValidator<ApplicationUser> _userValidator;
        private readonly IIdentityService _iIdentityService;

        public UpdateProfileCommandHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper,
            IValidator<ApplicationUser> userValidator,
            IIdentityService iIdentityService
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _userValidator = MethodParameterChecker.CheckUp(userValidator);
            _iIdentityService = MethodParameterChecker.CheckUp(iIdentityService);
        }

        public async Task<UserPatientDTO> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_iIdentityService.GetUserId().Value);

            user.Email = command.ObjectDTO.Email;
            user.NormalizedEmail = command.ObjectDTO.Email.ToUpper();
            user.MedicalLicenseNumber = command.ObjectDTO.MedicalLicenseNumber;

            await _userValidator.Validate(user);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(_mapper.ToDto(user));
        }
    }
}