namespace HotelManagement.Models
{
    public class MergeRRO
    {
        public Room Room { get; set; }

        public ReservationDetail Detail { get; set; }

        public Order Order { get; set; }

        public List<MenuItem> Menu { get; set; }

    }
}
