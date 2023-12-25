using Rhazes.Services.Identity.API.Application.DTO.IhioViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface IIhioService
    {
        Task<UserSessionDTO> GetUserSessionAsync(UserSessionRequestDTO data, CancellationToken cancellationToken);
        Task<GetPatientInsuranceDTO> GetPatientInsuranceAsync(GetPatientInsuranceRequestDTO data , CancellationToken cancellationToken);
    }
}
