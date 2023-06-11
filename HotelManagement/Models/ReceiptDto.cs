using HotelManagementAPI.Models;
using Newtonsoft.Json;

namespace HotelManagement.Models;

public class ReceiptDto
{
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("reservationDetail")] public ReservationDetail? ReservationDetail { get; set; }
    [JsonProperty("reservationDetailId")] public string? ReservationDetailId { get; set; }
    [JsonProperty("personnelId")] public string? PersonnelId { get; set; }
    [JsonProperty("personnel")] public Personnel? Personnel { get; set; }
    [JsonProperty("customer")] public Customer? Customer { get; set; }
    [JsonProperty("room")] public Room? Room { get; set; }
    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonProperty("orderAt")] public double OrderPrice { get; set; }
    [JsonProperty("totalPrice")] public double TotalPrice { get; set; }
}