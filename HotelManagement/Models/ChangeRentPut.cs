namespace HotelManagement.Models
{
    public class ChangeRentPut
    {
        public Room RoomRent { get; set; }

        public Room RoomChange { get; set; }

        public ReservationDetail ReDetail { get; set; }
    }
}
