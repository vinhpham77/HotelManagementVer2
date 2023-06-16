namespace HotelManagement.Models
{
    public class RentBookRoom
    {
        public Reservation Reservation { get; set; }

        public ReservationDetail ReservationDetail { get; set; }

        public Room? Room { get; set; }
    }
}
