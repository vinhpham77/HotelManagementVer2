namespace HotelManagement.Models
{
    public class BookRoom
    {

        public string Id { get; set; } = null!;
     
        public Reservation? Reservation { get; set; }
      
        public Customer? Customer { get; set; }

        public List<ReservationDetail>? ReservationDetail { get; set; } = null!;
    }

}
