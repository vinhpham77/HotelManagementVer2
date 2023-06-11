
using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class Room
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("roomTypeId")]
        public string RoomTypeId { get; set; } = null!;

        [JsonProperty("pricePerDay")]
        public double PricePerDay { get; set; }

        [JsonProperty("isEmpty")]
        public bool IsEmpty { get; set; }

        [JsonProperty("isCleaned")]
        public bool IsCleaned { get; set; }

        [JsonProperty("lastCleanedAt")]
        public DateTime LastCleanedAt { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; } = null!;

        [JsonProperty("maxAdult")]
        public int MaxAdult { get; set; }

        [JsonProperty("maxChild")]
        public int MaxChild { get; set; }
    }
}
