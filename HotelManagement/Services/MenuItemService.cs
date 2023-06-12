using System.Text;
using HotelManagement.Models;
using System.Web;
using Newtonsoft.Json;

namespace HotelManagement.Services
{
    public class MenuItemService
    {
        private readonly HttpClient _httpClient;
        private readonly string _menuApiUrl = "Menu";

        public MenuItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LCount<MenuItem>?> GetAsync()
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _menuApiUrl : $"{_menuApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LCount<MenuItem>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
    }
}
