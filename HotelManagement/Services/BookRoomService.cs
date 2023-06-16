using HotelManagement.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web;
using System.Text.Json;

namespace HotelManagement.Services
{
    public class BookRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bookApiUrl = "BookRoomDtos";

        public BookRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<BookRoom>?> GetAsync(string? keyword, int? page, int? size, DateTime? startDate, DateTime? endDate, Boolean? temp, Boolean? onlyReservedAt)
        {

            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["keyword"] = keyword;
            if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
            if (size.HasValue) queryBuilder["size"] = size.Value.ToString();
            queryBuilder["startDate"]= startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ");
            queryBuilder["endDate"]= endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ");
            if (temp.HasValue) queryBuilder["temp"]=temp.Value.ToString();   
            if (onlyReservedAt.HasValue) queryBuilder["onlyReservedAt"] = onlyReservedAt.Value.ToString();  
            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _bookApiUrl : $"{_bookApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BookRoom>?>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }

    }
}
