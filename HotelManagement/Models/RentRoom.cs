
using HotelManagement.Models;
using Newtonsoft.Json;

namespace HotelManagementAPI.Models
{
    public class RentRoom
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        public List<Room>? rooms { get; set; }
    }
}
