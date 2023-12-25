using IdentityServer4.Models;
using Rhazes.Services.Identity.API.Application.DTO.TenantViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface ITenantService
    {
        Task<Guid> CreateTenantByIhioAsync(CreateTenantByIhioDTO data, CancellationToken cancellationToken);
        Task<Guid> CreateTenantForLoginOTPAsync(CreateTenantForLoginOTPDTO data, CancellationToken cancellationToken);
        Task<bool> DeleteTenantAsync(Guid Id, Guid UserId, CancellationToken cancellationToken);
        Task<List<TenantDTO>> GetAllTenantIncludeDeletedByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<InvitationDTO> GetInvitationByIdAsync(Guid invitationId, CancellationToken cancellationToken);
        Task<UserTenantDTO> AddUserTenantAsync(UserTenantDTO userTenant, CancellationToken cancellationToken);
    }
}
