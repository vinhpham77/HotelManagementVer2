namespace HotelManagement.Models
{
    public class ReOrder
    {
        public string? Id { get; set; }

        public ReservationDetail? Detail { get; set; }

        public Order? Order { get; set; }
    }
}
