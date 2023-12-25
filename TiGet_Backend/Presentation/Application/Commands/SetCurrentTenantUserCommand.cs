using System;
using System.Runtime.Serialization;
using MediatR;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class SetCurrentTenantUserCommand : IRequest<bool>
    {
        public Guid TenantId { get; set; }
        public int UserTypeId { get; set; }
        public SetCurrentTenantUserCommand(
        Guid tenantId,
        int userTypeId

        )
        {
            TenantId = tenantId;
            UserTypeId = userTypeId;
         
        }
    }
}
