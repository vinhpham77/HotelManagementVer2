using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HotelManagement.Models;

public class RoomDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = null!;
    
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    
    [JsonProperty("roomTypeId")]
    public string RoomTypeId { get; set; } = null!;
    
    [JsonProperty("pricePerDay")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public double PricePerDay { get; set; }
    
    [JsonProperty("isEmpty")]
    public bool IsEmpty { get; set; }
    
    [JsonProperty("isCleaned")]
    public bool IsCleaned { get; set; }
    
    [JsonProperty("lastCleanedAt")]
    public DateTime LastCleanedAt { get; set; }
    
    [JsonProperty("description")]
    public string? Description { get; set; }
    
    [JsonProperty("maxAdult")]
    public int MaxAdult { get; set; }
    
    [JsonProperty("maxChild")]
    public int MaxChild { get; set; }
    
    [JsonProperty("roomType")]
    public RoomType RoomType { get; set; } = null!;
}