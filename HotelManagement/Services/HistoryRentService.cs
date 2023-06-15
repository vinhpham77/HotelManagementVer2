using HotelManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace HotelManagement.Services
{
    public class HistoryRentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _historyRentApiUrl = "HistoryRent";

        public HistoryRentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HistoryRent>?> GetAsync()
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _historyRentApiUrl : $"{_historyRentApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HistoryRent>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }

        public async Task<HistoryRent?> GetByIdAsync(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_historyRentApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<HistoryRent>(jsonResponse);
            }

            throw new HttpRequestException(
                $"Request to get RoomType by id {id} failed with status code: {response.StatusCode}");
        }

    }
}
