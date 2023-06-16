namespace HotelManagement.Models
{
	public class MyViewModel
	{
		
		public Reservation Reservation { get; set; }
		public List<Room> Rooms { get; set; }
		public Customer? Customer { get; set; }
	}
}
