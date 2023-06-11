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
    }
}
