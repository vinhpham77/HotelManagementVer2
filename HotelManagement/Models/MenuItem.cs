using Newtonsoft.Json;

namespace HotelManagement.Models;

public class MenuItem
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    
    [JsonProperty("type")]
    public bool? Type { get; set; }
    
    [JsonProperty("importPrice")]
    public double ImportPrice { get; set; }
    
    [JsonProperty("exportPrice")]
    public double ExportPrice { get; set; }
}