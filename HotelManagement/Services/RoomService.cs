using System.Text;
using System.Web;
using HotelManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace HotelManagement.Services
{
    public class RoomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _roomApiUrl = "rooms";
        private readonly string _roomDtoApiUrl = "roomDtos";

        public RoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<LCount<RoomDto>?> GetAsync(string? keyword, string? sort, string? order, int? page, int? size)
        {
            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder["keyword"] = keyword;
            queryBuilder["sort"] = sort;
            queryBuilder["order"] = order;
            if (page.HasValue) queryBuilder["page"] = page.Value.ToString();
            if (size.HasValue) queryBuilder["size"] = size.Value.ToString();

            string? queryString = queryBuilder.ToString();
            string requestUrl = string.IsNullOrEmpty(queryString) ? _roomDtoApiUrl : $"{_roomDtoApiUrl}?{queryString}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LCount<RoomDto>>(jsonResponse);
            }

            throw new HttpRequestException($"Request to {requestUrl} failed with status code: {response.StatusCode}");
        }
        

        public async Task<Room?> GetByIdAsync(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_roomApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Room>(jsonResponse);
            }

            throw new HttpRequestException(
                $"Request to get Room by id {id} failed with status code: {response.StatusCode}");
        }
        
        public async Task<Room?> CreateAsync(Room room)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_roomApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Room>(jsonResponse);
            }
        
            throw new HttpRequestException(
                $"Request to create Room failed with status code: {response.StatusCode}");
        }

        public async Task UpdateAsync(Room room)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_roomApiUrl}/{room.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update Room with id {room.Id} failed with status code: {response.StatusCode}");
            }
        }
        
        public async Task UpdateAsync(string id, Room room)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_roomApiUrl}/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update Room with id {room.Id} failed with status code: {response.StatusCode}");
            }
        }
        
        public async Task UpdateAsync(string id, JsonPatchDocument<Room> roomPatchDocument)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(roomPatchDocument), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync($"{_roomApiUrl}/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update Room with id {id} failed with status code: {response.StatusCode}");
            }
        }

        public async Task DeleteAsync(string id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_roomApiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to delete Room with id {id} failed with status code: {response.StatusCode}");
            }
        }

        public async Task DeleteManyAsync(string[] roomIds)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(roomIds), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _roomApiUrl)
            {
                Content = content
            };
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to delete multiple Rooms failed with status code: {response.StatusCode}");
            }
        }
    }
}