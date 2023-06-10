using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class Account
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [JsonProperty("role")]
        public string Role { get; set; } = null!;
        
        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}
