namespace HotelManagement.Models
{
    public class Book
    {
        public List<Room> Rooms { get; set; }
        public List<BookRoom> BookRooms { get; set; }
        public List<MergeCD> MergeCDs { get; set; }

        public DateTime Day { get; set; }
        public DateTime EndDay { get; set; }
    }
}
