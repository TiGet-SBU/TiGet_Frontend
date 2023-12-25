using FluentValidation;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Validations
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        private readonly IApplicationUserManager _userManager;
        public DeleteRoleCommandValidator(
            IApplicationUserManager userManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(userManager);
            RuleFor(command => command.Id).NotEqual(Guid.Empty).WithMessage("ID_IS_REQUIRED")
                .Must(x=> CheckUseRole(x)).WithMessage("ROLE_USE_IN_USER");
        }

        public bool CheckUseRole(Guid id)
        {
            if (id != Guid.Empty)
            {
                return !_userManager.UserRoleAsync(id).Result;
            }
            return true;
        }


    }
}
