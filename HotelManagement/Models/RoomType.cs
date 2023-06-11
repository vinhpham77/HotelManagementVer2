
using Newtonsoft.Json;

namespace HotelManagement.Models;

public class RoomType
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    
    [JsonProperty("description")]
    public string Description { get; set; } = null!;
}