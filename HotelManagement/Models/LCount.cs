using Newtonsoft.Json;

namespace HotelManagement.Models;

public class LCount<T>
{
    [JsonProperty("items")]
    public List<T> Items { get; set; } = default!;
    
    [JsonProperty("total")]
    public long Total { get; set; }
}