
using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class Reservation
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("roomIds")]
        
        public string[] RoomIds { get; set; } = Array.Empty<string>();

        [JsonProperty("customerId")]
        
        public string? CustomerId { get; set; }

        [JsonProperty("personnelId")]
        
        public string PersonnelId { get; set; } = null!;

        [JsonProperty("reservedAt")]
        public DateTime ReservedAt { get; set; }

        [JsonProperty("deposit")]
        public double Deposit { get; set; }

        [JsonProperty("checkedOut")]
        public DateTime CheckedOut { get; set; }
    }
}

