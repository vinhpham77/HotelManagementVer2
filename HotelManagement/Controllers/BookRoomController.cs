using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace HotelManagement.Controllers
{
    public class BookRoomController : Controller
    {

        private readonly RoomService _roomService;
        private readonly ReservationDetailService _reservationDetailService;
        private readonly ReservationService _reservationService;
        private readonly BookRoomService _bookRoomService;
        private readonly MergeCDService _mergeService;
        private readonly ILogger<BookRoomController> _logger;
        public BookRoomController(
            RoomService roomService,
          ReservationDetailService reservationDetailService,
          ILogger<BookRoomController> logger,
          BookRoomService bookRoomService,
          MergeCDService mergeService)
          
        {
                   
            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _logger = logger;
            _bookRoomService = bookRoomService;
            _mergeService = mergeService;
        }
        public async Task<IActionResult> Index(string? key, DateTime? startDate, bool? temp, int? page, int? size)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now;
            }
            if(!temp.HasValue)
            {
                temp = true;
            }
            DateTime endDay = startDate.Value.AddDays(6); // Cộng thêm 6 ngày
                var bookRoom = await _bookRoomService.GetAsync(key, page, size, startDate, endDay, temp);
                LCount<Room>? room = await _roomService.GetAllAsync();
                 var mergeCD = await _mergeService.GetAsync(startDate, endDay);
            var data = new Book
                {
                    Rooms = room.Items,
                    BookRooms = bookRoom,
                    MergeCDs=mergeCD,
                    Day = startDate.Value,
                    EndDay = endDay
            };

            return View(data);
        }



        public IActionResult Add()
        {

            return PartialView("Add"); // Trả về partial view (file .cshtml) chứa form đăng kí
        }
    }
}
