using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class ReservationDetail
    {
        
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("reservationId")]
        public string? ReservationId { get; set; }

        [JsonProperty("roomId")]
        public string? RoomId { get; set; }

        [JsonProperty("checkedInAt")]
        public DateTime CheckedInAt { get; set; }

        [JsonProperty("checkedOutAt")]
        public DateTime? CheckedOutAt { get; set; }

        [JsonProperty("customerId")]
        public string? CustomerId { get; set; }

        [JsonProperty("deposit")]
        public double Deposit { get; set; }

        [JsonProperty("totalAdults")]
        public int TotalAdults { get; set; }

        [JsonProperty("totalChildren")]
        public int TotalChildren { get; set; }

        [JsonProperty("roomPricePerDay")]
        public double RoomPricePerDay { get; set; }
    }
}
