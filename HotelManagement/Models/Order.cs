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

        public double TotalPrice(Order order)
        {
            double total = 0;
            foreach(OrderDetail item in order.Details)
            {
                total += item.Price * item.Quantity;
            }
            return Math.Floor(total);
        }
    }
}
