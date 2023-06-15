namespace HotelManagement.Models
{
    public class BookRoom
    {
        
        public string Id { get; set; } = null!;
        public DateTime ReservedAt { get; set; }
        public DateTime CheckedOut { get; set; }
        public Customer? Customer { get; set; }


        public string[] RoomIds { get; set; } = null!;

        public List<ReservationDetail> ReservationDetails { get; set; } = null!;
    }
}
