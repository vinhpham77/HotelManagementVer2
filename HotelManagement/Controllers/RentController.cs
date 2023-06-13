using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index(string? keyword, string? sort, string? order)
        {
            try
            {
                var rentRooms = await _rentRoomService.GetAsync(keyword, sort, order);
                var reOrder = await _reOrderService.GetAsync(false);
                var data = new Rent {
                    RentRooms = rentRooms,
                    ReOrders = reOrder
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
            return View(data);
        }

        public IActionResult BottomMenu()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult ChangeRoom()
        {
            return View();
        }

        public IActionResult Check()
        {
            return View();
        }
    }
}
