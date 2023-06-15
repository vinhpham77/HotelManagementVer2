
using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class Receipt
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("personnelId")]
        public string? PersonnelId { get; set; }

        [JsonProperty("reservationDetailId")]
        public string? ReservationDetailId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("orderPrice")]
        public double OrderPrice { get; set; }

        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }

        public int daysIn(DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end.Subtract(start);
            return (int)(timeSpan.TotalDays);
        }
    }
}
