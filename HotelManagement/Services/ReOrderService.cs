using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;
using HotelManagement.Models;

namespace HotelManagement.Services
{
    public class ReOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _reOrderApiUrl = "ReOrder";

        public ReOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ReOrder>?> GetAsync(bool? checkedOut)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["checkedOut"] = checkedOut.ToString();

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _reOrderApiUrl : $"{_reOrderApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ReOrder>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
    }
}
