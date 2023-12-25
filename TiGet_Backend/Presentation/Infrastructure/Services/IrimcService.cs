using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.Services.Identity.API.Application.DTO.IrimcViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class IrimcService : IIrimcService
    {
        private readonly HttpClient _apiClient;
        private readonly IConfiguration _configuration;
        public static string GetMemberByNationalCodeAsync(string nationalCode) => $"/api/v1/ApiLand/GetMemberInfoByNationalCode/{nationalCode}";
        public static string GetMemberByMcCodeAsync(string mcCode) => $"/api/v1/ApiLand/GetMemberByMcCode/{mcCode}";
        public static string CheckValidMemberInMedicalCouncilAsync(string nationalCode, string mcCode, string phoneNumber) => $"/api/v1/ApiLand/CheckValidMemberInMedicalCouncil/{nationalCode}/{mcCode}/{mcCode}";
        public IrimcService(IConfiguration configuration, HttpClient apiClient)
        {
            _configuration = configuration;
            _apiClient = apiClient;
        }
        public async Task<MinimalMemberInfoDTO> GetMemberInfoByNationalCodeAsync(GetMemberByNationalCodeDTO data, CancellationToken cancellationToken)
        {
            try
            {
                var uri = _configuration["IrimcApiClient"] + GetMemberByNationalCodeAsync(data.NationalCode);
                var response = await _apiClient.GetAsync(uri, cancellationToken);
                response.EnsureSuccessStatus();
                var result = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<MinimalMemberInfoDTO>(result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<MemberInfoDTO> GetMemberByMcCodeAsync(GetMemberByMcCodeDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["IrimcApiClient"] + GetMemberByMcCodeAsync(data.McCode);
            var response = await _apiClient.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatus();
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<MemberInfoDTO>(result);
        }

        public async Task<bool> CheckValidMemberInMedicalCouncilAsync(CheckMemberMobileNumberDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["IrimcApiClient"] + CheckValidMemberInMedicalCouncilAsync(data.NationalCode,data.MedicalLicenseNumber,data.PhoneNumber);
            var response = await _apiClient.GetAsync(uri, cancellationToken);
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<bool>(result);
        }


    }
}
