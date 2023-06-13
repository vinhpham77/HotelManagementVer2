﻿using Newtonsoft.Json;

namespace HotelManagement.Models;

public class Personnel
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("username")] public string Username { get; set; } = null!;

    [JsonProperty("firstName")] public string FirstName { get; set; } = null!;

    [JsonProperty("lastName")] public string LastName { get; set; } = null!;

    [JsonProperty("fullName")] public string FullName { get; set; } = null!;
    
    [JsonProperty("phoneNumber")] public string? PhoneNumber { get; set; } = null!;

    [JsonProperty("idNo")] public string IdNo { get; set; } = null!;

    [JsonProperty("nationality")] public string Nationality { get; set; } = null!;

    [JsonProperty("sex")] public bool Sex { get; set; }

    [JsonProperty("birthdate")] public DateTime Birthdate { get; set; }
}