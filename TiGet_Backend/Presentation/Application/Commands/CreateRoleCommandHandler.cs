using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.Domain.Validators;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper<ApplicationRole, RoleDTO> _mapper;
        private readonly IValidator<ApplicationRole> _roleValidator;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper<ApplicationRole, RoleDTO> mapper, IValidator<ApplicationRole> roleValidator)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _roleValidator = MethodParameterChecker.CheckUp(roleValidator);
        }

        public async Task<RoleDTO> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var applicationRole = _mapper.ToEntity(command.ObjectDTO);

            await _roleValidator.Validate(applicationRole);

            var response = await _roleRepository.AddAsync(applicationRole);

            await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return await Task.FromResult(_mapper.ToDto(response));

        }

    }





}

