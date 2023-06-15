namespace HotelManagement.Models
{
    public class MergeCD
    {
       
        public string Id { get; set; } = null!;

        public ReservationDetail reservationDetail { get; set; }
        public Customer customer { get; set; }
    }
}
