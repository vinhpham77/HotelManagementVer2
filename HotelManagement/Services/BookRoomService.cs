using HotelManagement.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web;

namespace HotelManagement.Services
{
    public class BookRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bookApiUrl = "Book";

        public BookRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<BookRoom>?> GetAsync(string? keyword, int? page, int? size, DateTime? startDate, DateTime? endDate, Boolean? temp)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["keyword"] = keyword;
            if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
            if (size.HasValue) queryBuilder["size"] = size.Value.ToString();
            if(startDate!=DateTime.MinValue) queryBuilder["startDate"]=startDate.ToString();
            if(endDate!=DateTime.MinValue) queryBuilder["endDate"]=endDate.ToString();
            if(temp.HasValue) queryBuilder["temp"]=temp.Value.ToString();   

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
