using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.Services.Identity.API.Application.DTO.TenantViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly HttpClient _apiClient;
        private readonly IConfiguration _configuration;
        public static string CreateTenantByIhioAsync() => $"/api/v1/Tenant/CreateTenantByIhio";
        public static string CreateTenantForLoginOTPAsync() => $"/api/v1/Tenant/CreateTenantForLoginOTP";
        public static string DeleteTenantAsync() => $"/api/v1/Tenant/Delete";
        public static string GetAllTenantIncludeDeletedByUserIdAsync(Guid userId) => $"/api/v1/Tenant/GetAllTenantIncludeDeletedByUserId/{userId}";
        public static string GetInvitationByIdAsync(Guid invitationId) => $"/api/v1/Tenant/GetInvitationById/{invitationId}";
        public static string AddUserTenantAsync() => $"/api/v1/Tenant/AddUserTenant";
        public TenantService(IConfiguration configuration, HttpClient apiClient)
        {
            _configuration = configuration;
            _apiClient = apiClient;
        }
     
        public async Task<Guid> CreateTenantByIhioAsync(CreateTenantByIhioDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + CreateTenantByIhioAsync();
            var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var response = await _apiClient.PostAsync(uri, content, cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid>(result);
        }

        public async Task<Guid> CreateTenantForLoginOTPAsync(CreateTenantForLoginOTPDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + CreateTenantForLoginOTPAsync();
            var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var response = await _apiClient.PostAsync(uri, content, cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid>(result);
        }
        

        public async Task<bool> DeleteTenantAsync(Guid Id, Guid UserId, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + DeleteTenantAsync();
            var content = new StringContent(content: JsonConvert.SerializeObject(new { Id = Id, UserId = UserId }), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var response = await _apiClient.PostAsync(uri, content, cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }


        public async Task<List<TenantDTO>> GetAllTenantIncludeDeletedByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + GetAllTenantIncludeDeletedByUserIdAsync(userId);
            var data = await _apiClient.GetStringAsync(uri);
            var Tenants = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<List<TenantDTO>>(data) : null;

            return Tenants;
        }

        public async Task<InvitationDTO> GetInvitationByIdAsync(Guid invitationId, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + GetInvitationByIdAsync(invitationId);
            var data = await _apiClient.GetStringAsync(uri);
            var invitation = !string.IsNullOrEmpty(data) ? JsonConvert.DeserializeObject<InvitationDTO>(data) : null;

            return invitation;
        }

        public async Task<UserTenantDTO> AddUserTenantAsync(UserTenantDTO data, CancellationToken cancellationToken)
        {
            var uri = _configuration["TenantApiClient"] + AddUserTenantAsync();
            var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            var response = await _apiClient.PostAsync(uri, content, cancellationToken);

            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserTenantDTO>(result);
        }
    }
}
