using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;
using HotelManagement.Models;

namespace HotelManagement.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _orderApiUrl = "orders";

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ReservationDetail>?> GetAsync(string? reservationDetailId)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["reservationDetailId"] = reservationDetailId;

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _orderApiUrl : $"{_orderApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ReservationDetail>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
    }
}
