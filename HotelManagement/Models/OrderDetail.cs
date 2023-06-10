using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class OrderDetail
    {
        [JsonProperty("itemId")]
        public string? ItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("orderedAt")]
        public DateTime OrderedAt { get; set; }
    }
}
