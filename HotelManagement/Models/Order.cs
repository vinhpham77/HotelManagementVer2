using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class Order
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("reservationDetailId")]
        public string? ReservationDetailId { get; set; }

        [JsonProperty("details")]
        public OrderDetail[]? Details { get; set; } = null;
    }
}
