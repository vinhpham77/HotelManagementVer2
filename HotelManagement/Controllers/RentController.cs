using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HotelManagement.Controllers
{
    public class RentController : Controller
    {
        private readonly RentRoomService _rentRoomService;
        private readonly ReOrderService _reOrderService;
        private readonly RoomService _roomService;
        private readonly ReservationDetailService _reservationDetailService;
        private readonly OrderService _orderService;
        private readonly MenuItemService _menuItemService;
        private readonly ILogger<RentController> _logger;

        public RentController(RentRoomService rentRoomService, 
            ReOrderService reOrderService,
            RoomService roomService,
            ReservationDetailService reservationDetailService,
            OrderService orderService,
            MenuItemService menuItemService,
            ILogger<RentController> logger)
        {
            _rentRoomService = rentRoomService;
            _reOrderService = reOrderService;
            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _orderService = orderService;
            _menuItemService = menuItemService;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string? keyword, string? sort, string? order, string? tap)
        {
            try
            {
                var rentRooms = await _rentRoomService.GetAsync(keyword, sort, order);
                var reOrder = await _reOrderService.GetAsync(false);
                var data = new Rent {
                    RentRooms = rentRooms,
                    ReOrders = reOrder,
                    Tap = tap
                };
                return View(data);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving room types");
                return View("Error");
            }
        }

        public async Task<IActionResult> Edit(string roomId)
        {
            var room = await _roomService.GetByIdAsync(roomId);
            var reservationDetail = await _reservationDetailService.GetAsync(roomId, false);
            var detail = reservationDetail.Items.FirstOrDefault();
            var order = await _orderService.GetAsync(detail.Id);
            var menu = await _menuItemService.GetAsync();
            var data = new MergeRRO
            {
                Room = room,
                Detail = detail,
                Order = order.FirstOrDefault(),
                Menu = menu.Items
            };
            return PartialView(data);
        }

        public async Task<IActionResult> Order(string roomId)
        {
            var room = await _roomService.GetByIdAsync(roomId);
            var reservationDetail = await _reservationDetailService.GetAsync(roomId, false);
            var detail = reservationDetail.Items.FirstOrDefault();
            var order = await _orderService.GetAsync(detail.Id);
            var menu = await _menuItemService.GetAsync();
            var data = new MergeRRO
            {
                Room = room,
                Detail = detail,
                Order = order.FirstOrDefault(),
                Menu = menu.Items
            };
            return PartialView(data);
        }

        public async Task<IActionResult> ChangeRoom(string roomId)
        {
            try
            {
                var rentRooms = await _rentRoomService.GetAsync(null, null, null);
                var room = await _roomService.GetByIdAsync(roomId);
                var reservationDetail = await _reservationDetailService.GetAsync(roomId, false);
                var data = new ChangeRent
                {
                    RentRooms = rentRooms,
                    RoomSelect = room,
                    Detail = reservationDetail.Items.FirstOrDefault()
            };
                return PartialView(data);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving room types");
                return View("Error");
            }
        }

        public async Task<IActionResult> Check(string roomId)
        {
            var room = await _roomService.GetByIdAsync(roomId);
            var reservationDetail = await _reservationDetailService.GetAsync(roomId, false);
            var detail = reservationDetail.Items.FirstOrDefault();
            var order = await _orderService.GetAsync(detail.Id);
            var data = new MergeRRO
            {
                Room = room,
                Detail = detail,
                Order = order.FirstOrDefault()
            };
            return PartialView(data);
        }

        [HttpPut]
        public async Task<JsonResult> EditPut([FromBody] ReOrder Edit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Edit.Detail.CheckedInAt = Edit.Detail.CheckedInAt.ToUniversalTime();
                    await _reservationDetailService.UpdateAsync(Edit.Detail);
                    await _orderService.UpdateAsync(Edit.Order);
                    return Json(Edit);
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<JsonResult> OrderPut([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateAsync(order);
                    return Json(new { success = true });
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<JsonResult> ChangeRoomPut([FromBody] ChangeRentPut data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    data.RoomChange.LastCleanedAt = data.RoomChange.LastCleanedAt.ToUniversalTime();
                    await _roomService.UpdateAsync(data.RoomChange);
                    await _roomService.UpdateAsync(data.RoomRent);
                    await _reservationDetailService.UpdateAsync(data.ReDetail);
                    return Json(new { success = true });
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        [HttpPut]
        public async Task<JsonResult> UpdateRoom([FromBody] Room room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roomService.UpdateAsync(room);
                    return Json(new { success = true });
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }
    }
}
