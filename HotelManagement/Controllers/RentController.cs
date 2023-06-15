using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Dependency;
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
        private readonly ReceiptService _receiptService;
        private readonly HistoryRentService _historyRentService;
        private readonly ILogger<RentController> _logger;

        public RentController(RentRoomService rentRoomService,
            ReOrderService reOrderService,
            RoomService roomService,
            ReservationDetailService reservationDetailService,
            OrderService orderService,
            MenuItemService menuItemService,
            ReceiptService receiptService,
            HistoryRentService historyRentService,
            ILogger<RentController> logger)
        {
            _rentRoomService = rentRoomService;
            _reOrderService = reOrderService;
            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _orderService = orderService;
            _menuItemService = menuItemService;
            _receiptService = receiptService;
            _historyRentService = historyRentService;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string? keyword, string? sort, string? order, string? tap)
        {
            try
            {
                if (string.IsNullOrEmpty(sort))
                    sort = "name";
                if (string.IsNullOrEmpty(order))
                    order = "asc";
                var rentRooms = await _rentRoomService.GetAsync(keyword, sort, order);
                var reOrder = await _reOrderService.GetAsync(false);
                var history = await _historyRentService.GetAsync();
                var data = new Rent {
                    RentRooms = rentRooms,
                    ReOrders = reOrder,
                    Tap = tap,
                    History = history,
                    Order = order
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

        public async Task<IActionResult> UpdateHistory(string receiptId)
        {
            HistoryRent history = await _historyRentService.GetByIdAsync(receiptId);
            return PartialView(history);
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

        [HttpGet]
        public async Task<JsonResult> GetById(string id)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            return Json(receipt);
        }

        [HttpPut]
        public async Task<IActionResult> CheckOut([FromBody] MergeRRO merge)
        {
            try
            {
                var orderPrice = merge.Order.TotalPrice(merge.Order);
                var priceRoom = merge.Detail.roomPiceDay(merge.Detail, merge.Detail.CheckedInAt, merge.Detail.CheckedOutAt.Value);
                var roomSurcharge = merge.Detail.roomSurcharge(merge.Detail, merge.Room, merge.Detail.CheckedInAt, merge.Detail.CheckedOutAt.Value);
                Receipt receipt = new Receipt
                {
                    PersonnelId = merge.Detail.CustomerId,
                    ReservationDetailId = merge.Detail.Id,
                    CreatedAt = DateTime.Now,
                    OrderPrice = orderPrice,
                    TotalPrice = orderPrice + priceRoom + roomSurcharge
                };
                merge.Detail.CheckedInAt = merge.Detail.CheckedInAt.ToUniversalTime();
                merge.Detail.CheckedOutAt = merge.Detail.CheckedOutAt.Value.ToUniversalTime();

                await _roomService.UpdateAsync(merge.Room);
                await _reservationDetailService.UpdateAsync(merge.Detail);
                await _receiptService.CreateAsync(receipt);
                return Json(new { success = true });
            }
            catch (HttpRequestException)
            {
                return Json(new { success = false });
            }
        }

        [HttpPut]
        public async Task<JsonResult> UpdateHistory([FromBody] HistoryRent history)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    history.Detail.CheckedInAt = history.Detail.CheckedInAt.ToUniversalTime();
                    history.Detail.CheckedOutAt = history.Detail.CheckedOutAt.Value.ToUniversalTime();
                    await _receiptService.UpdateAsync(history.Receipt);
                    await _reservationDetailService.UpdateAsync(history.Detail);
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
