using Rhazes.Services.Identity.API.Application.DTO.IhioViewModels;
using Rhazes.Services.Identity.API.Application.DTO.IrimcViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface IIrimcService
    {
        //Task<MemberInfoDTO> GetMemberByNationalCodeAsync(GetMemberByNationalCodeDTO data, CancellationToken cancellationToken);
        //Task<MemberInfoDTO> GetMemberByMcCodeAsync(GetMemberByMcCodeDTO data, CancellationToken cancellationToken);
        //Task<bool> CheckMemberMobileNumberAsync(CheckMemberMobileNumberDTO data, CancellationToken cancellationToken);

        //ApiLand
        Task<MinimalMemberInfoDTO> GetMemberInfoByNationalCodeAsync(GetMemberByNationalCodeDTO data, CancellationToken cancellationToken);
        Task<MemberInfoDTO> GetMemberByMcCodeAsync(GetMemberByMcCodeDTO data, CancellationToken cancellationToken);
        Task<bool> CheckValidMemberInMedicalCouncilAsync(CheckMemberMobileNumberDTO data, CancellationToken cancellationToken);
    }
}
