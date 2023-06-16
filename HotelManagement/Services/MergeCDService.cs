using HotelManagement.Models;
using Newtonsoft.Json;
using System.Web;

namespace HotelManagement.Services
{
    public class MergeCDService
    {
        private readonly HttpClient _httpClient;
        private readonly string _mergeCDApiUrl = "MergeCD";

        public MergeCDService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<MergeCD>?> GetAsync(DateTime? startDate, DateTime? endDate)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            
           
            if (startDate != DateTime.MinValue) queryBuilder["startDate"] = startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ");
            if (endDate != DateTime.MinValue) queryBuilder["endDate"] = endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ");

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _mergeCDApiUrl : $"{_mergeCDApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MergeCD>?>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
    }
}
