
using Rhazes.BuildingBlocks.EventBus.Events;
using System;

namespace Rhazes.Services.Identity.API.IntegrationEvents.Events
{
    public record AddUserRoleIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string Role { get; set; }

        public AddUserRoleIntegrationEvent(
        Guid userId,
        Guid tenantId,
        string role
            )

        {
            UserId = userId;
            TenantId = tenantId;
            Role = role;
        }
    }
}
