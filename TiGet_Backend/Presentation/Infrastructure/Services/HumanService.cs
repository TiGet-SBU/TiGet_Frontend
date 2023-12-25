using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class HumanService : IHumanService
    {
        private readonly HttpClient _apiClient;
        private readonly IConfiguration _configuration;
        public static string AddStaffByIhioAsync() => $"/api/v1/Staff/AddByIhio";
        public static string DeleteStaffAsync() => $"/api/v1/Staff/Delete";
        public HumanService(IConfiguration configuration, HttpClient apiClient)
        {
            _configuration = configuration;
            _apiClient = apiClient;
        }

        public async Task<StaffDTO> AddStaffAsync(StaffRequestDTO data, CancellationToken cancellationToken)
        {
            StaffDTO staff = new StaffDTO();
            try
            {
                var uri = _configuration["HumanApiClient"] + AddStaffByIhioAsync();

                var content = new StringContent(content: JsonConvert.SerializeObject(data), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _apiClient.PostAsync(uri, content);

                response.EnsureSuccessStatus();

                var staffAddResponse = await response.Content.ReadAsStringAsync();
                staff.Id = JsonConvert.DeserializeObject<Guid>(staffAddResponse);
                return staff;
            }
            catch (Exception ex)
            {
                staff.Message = ex.Message.ToString();
                return staff;
            }

        }

        public async Task<bool> DeleteStaffAsync(Guid Id, CancellationToken cancellationToken)
        {
            var uri = _configuration["HumanApiClient"] + DeleteStaffAsync();

            var content = new StringContent(content: JsonConvert.SerializeObject(Id), encoding: System.Text.Encoding.UTF8, mediaType: "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _apiClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();

            var staffAddResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(staffAddResponse);
        }
    }
}
