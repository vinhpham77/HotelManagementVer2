using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class RentController : Controller
    {
        private readonly RentRoomService _rentRoomService;
        private readonly ReOrderService _reOrderService;
        private readonly ILogger<RentController> _logger;

        public RentController(RentRoomService rentRoomService, ReOrderService reOrderService, ILogger<RentController> logger)
        {
            _rentRoomService = rentRoomService;
            _reOrderService = reOrderService;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string? keyword, string? sort, string? order, Boolean? checkedOut)
        {
            try
            {
                var rentRooms = await _rentRoomService.GetAsync(keyword, sort, order);
                var reOrder = await _reOrderService.GetAsync(checkedOut);
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

        public IActionResult Edit(string id)
        {
            var data = new { Id = id};
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
