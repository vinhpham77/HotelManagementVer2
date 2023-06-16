using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Drawing;

using Microsoft.CodeAnalysis.Differencing;

using System.Runtime.InteropServices;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HotelManagement.Controllers
{
    public class BookRoomController : Controller
    {

        private readonly RoomService _roomService;
        private readonly ReservationDetailService _reservationDetailService;
        private readonly ReservationService _reservationService;
        private readonly BookRoomService _bookRoomService;
        private readonly MergeCDService _mergeService;
        private readonly CustomerService _customerService;
        private readonly ILogger<BookRoomController> _logger;
		
		public BookRoomController(
            RoomService roomService,
          ReservationDetailService reservationDetailService,
          ILogger<BookRoomController> logger,
          BookRoomService bookRoomService,
          MergeCDService mergeService, CustomerService customerService,
          ReservationService reservationService)
          
        {
                   
            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _logger = logger;
            _bookRoomService = bookRoomService;
            _mergeService = mergeService;
            _customerService = customerService;
            _reservationService = reservationService;
               
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

                var bookRoom = await _bookRoomService.GetAsync(key, page, size, startDate, endDay, temp, null);
                LCount<Room>? room = await _roomService.GetAllAsync();
                 var mergeCD = await _mergeService.GetAsync(startDate, endDay);

            var data = new Book
            {
                Rooms = room.Items,
                BookRooms = bookRoom,
                MergeCDs = mergeCD,
                Day = startDate.Value,
                EndDay = endDay     
            };

            return View(data);
        }


        
     
        public async Task<IActionResult> Add()
        {
            var room=await _roomService.GetAllAsync();

            return PartialView(room.Items); // Trả về partial view (file .cshtml) chứa form đăng kí
        }
        public async Task<JsonResult> GetCustomerById(string idNo)
        {
            LCount<Customer>? customer = await _customerService.GetAsync(null, null, null, null, null, idNo);
            return Json(customer.Items.FirstOrDefault());
        } 

        public async Task<JsonResult> GetReservation(DateTime startDate, DateTime endDate, Boolean onlyReservedAt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = await _bookRoomService.GetAsync("",null,null, startDate, endDate, null,onlyReservedAt);
                    return Json(data);
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

		public async Task<JsonResult>Quoc(DateTime startDate, DateTime endDate)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var bookRoom = await _bookRoomService.GetAsync("", null, null, startDate, endDate, true, null);
					var mergeCD = await _mergeService.GetAsync(startDate, endDate);
					return Json(new { bookRoom, mergeCD });
				}
				catch (HttpRequestException)
				{
					return Json(new { success = false });
				}
			}
			return Json(new { success = false });
		}
		public async Task<IActionResult> Update(string? id)
		{
			var roomItems = await _roomService.GetAllAsync();
			var reservation = await _reservationService.GetByIdAsync(id);
            var customer=await _customerService.GetByIdAsync(reservation.CustomerId);


            var viewModel = new MyViewModel
            {
                Rooms = roomItems.Items,
                Reservation = reservation,
                Customer = customer
		};
			return PartialView(viewModel);
		}

		//public async Task<JsonResult> GetVal(DateTime? startDate, DateTime? endDate)
		//{
		//	var bookRoom = await _bookRoomService.GetAsync("", null, null, startDate, endDate, true, null);
		//	var mergeCD = await _mergeService.GetAsync(startDate, endDate);
		//	return Json(new { bookRoom, mergeCD });



		//}
	}
}
