using HotelManagementAPI.Models;
using System.Security.Cryptography.X509Certificates;

namespace HotelManagement.Models
{
    public class RentBook
    {
        public Reservation reservation { get; set; }
        public List<RentRoom> RentRooms { get; set; }
    }
}
