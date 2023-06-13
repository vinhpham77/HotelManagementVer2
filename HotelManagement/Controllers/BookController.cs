﻿using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Add()
		{
           
			return PartialView("Add"); // Trả về partial view (file .cshtml) chứa form đăng kí
		}
	}
}
