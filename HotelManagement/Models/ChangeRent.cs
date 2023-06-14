using HotelManagementAPI.Models;

namespace HotelManagement.Models
{
    public class ChangeRent
    {
        public List<RentRoom>? RentRooms { get; set; }
        public Room? RoomSelect { get; set; }
        public ReservationDetail? Detail { get; set; }
    }
}
