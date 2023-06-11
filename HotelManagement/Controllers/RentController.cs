using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class RentController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
