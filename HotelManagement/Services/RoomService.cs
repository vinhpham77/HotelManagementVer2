using System.Web;
using Newtonsoft.Json;
using HotelManagementAPI.Models;
using HotelManagement.Models;
using System.Text;

namespace HotelManagement.Services
{
    public class RoomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _roomApiUrl = "rooms";

        public RoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                $"Request to get RoomType by id {id} failed with status code: {response.StatusCode}");
        }

        public async Task UpdateAsync(Room room)
        {
            StringContent content =
                new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"{_roomApiUrl}/{room.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Request to update RoomType with id {room.Id} failed with status code: {response.StatusCode}");
            }
        }
    }
}
