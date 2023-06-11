using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;

namespace HotelManagement.Services
{
    public class RentRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _rentRoommApiUrl = "RentRooms";

        public RentRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RentRoom>?> GetAsync(string? keyword, string? sort, string? order)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["keyword"] = keyword;
            queryBuilder["sort"] = sort;
            queryBuilder["order"] = order;

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _rentRoommApiUrl : $"{_rentRoommApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RentRoom>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
    }
}
