using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HotelManagement.Models
{
    public class HistoryRent
    {
        public string? Id { get; set; }

        public string? NameRoom { get; set; }

        public Receipt? Receipt { get; set; }

        public ReservationDetail? Detail { get; set; }
    }
}
