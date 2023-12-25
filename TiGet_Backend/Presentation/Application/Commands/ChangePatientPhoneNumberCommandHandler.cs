using MediatR;
using Rhazes.BuildingBlocks.Common;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class ChangePatientPhoneNumberCommandHandler : IRequestHandler<ChangePatientPhoneNumberCommand, bool>
    {
        private readonly IApplicationUserManager _userManager;

        public ChangePatientPhoneNumberCommandHandler(
            IApplicationUserManager userManager
            )
        {
            _userManager =  MethodParameterChecker.CheckUp(userManager);
        }

        public async Task<bool> Handle(ChangePatientPhoneNumberCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.NationalCode);
            if (user.UserType == UserType.Patient.Id)
            {
                user.PhoneNumber = command.PhoneNumber;
                user.ModifiedById = command.ModifiedById;
                user.ModifiedDate = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }
            return true;
        }
    }

  
}
