using Newtonsoft.Json;

namespace HotelManagement.Models;

public class LoginRequest
{
    [JsonProperty("username")]
    public string Username { get; set; } = null!;
    
    [JsonProperty("password")]
    public string Password { get; set; } = null!;
}