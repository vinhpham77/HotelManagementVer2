using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;
using HotelManagement.Models;
using System.Text;

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

        public async Task<List<Order>?> GetAsync(string? reservationDetailId)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["reservationDetailId"] = reservationDetailId;

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _orderApiUrl : $"{_orderApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }

        public async Task UpdateAsync(Order order)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_orderApiUrl}/{order.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update RoomType with id {order.Id} failed with status code: {response.StatusCode}");
            }
        }
    }
}
