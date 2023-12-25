using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.Services.Identity.API.Application.DTO.IhioViewModels;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class IhioService : IIhioService
    {
        private readonly HttpClient _apiClient;
        private readonly IConfiguration _configuration;
        public static string GetUserSessionAsync() => $"/api/v1/Session/GetUserSession";
        public static string GetPatientInsuranceAsync() => $"/api/v1/Session/GetPatientInsurance";
        public IhioService(IConfiguration configuration, HttpClient apiClient)
        {
            _configuration = configuration;
            _apiClient = apiClient;
        }
        public async Task<UserSessionDTO> GetUserSessionAsync(UserSessionRequestDTO data, CancellationToken cancellationToken)
        {
            try
            {
                var response = new HttpResponseMessage();
                var uri = _configuration["IhioApiClient"] + GetUserSessionAsync();
                var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                response = await _apiClient.PostAsync(uri, content, cancellationToken);
                response.EnsureSuccessStatus();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserSessionDTO>(result);
            }
            catch (Exception ex)
            {
                var userSession= new UserSessionDTO();
                userSession.Message = ex.Message.ToString();
                return userSession;
            }
        

        }

        public async Task<GetPatientInsuranceDTO> GetPatientInsuranceAsync(GetPatientInsuranceRequestDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["IhioApiClient"] + GetPatientInsuranceAsync();
            var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var response = await _apiClient.PostAsync(uri, content, cancellationToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetPatientInsuranceDTO>(result);
        }
    }
}
