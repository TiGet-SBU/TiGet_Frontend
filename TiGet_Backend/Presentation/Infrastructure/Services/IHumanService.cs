using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface IHumanService
    {
        Task<StaffDTO> AddStaffAsync(StaffRequestDTO data, CancellationToken cancellationToken);
        Task<bool> DeleteStaffAsync(Guid Id, CancellationToken cancellationToken);
    }
}
