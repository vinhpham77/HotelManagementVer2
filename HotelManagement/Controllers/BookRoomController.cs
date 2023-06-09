﻿using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class BookRoomController : Controller
    {
        private readonly RentRoomService _rentRoomService;
        private readonly RoomService _roomService;
        private readonly ReservationDetailService _reservationDetailService;
        private readonly ReservationService _reservationService;
        private readonly BookRoomService _bookRoomService;
        private readonly MergeCDService _mergeService;
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly ILogger<BookRoomController> _logger;

        public BookRoomController(RentRoomService rentRoomService,
            RoomService roomService,
            ReservationDetailService reservationDetailService,
            ILogger<BookRoomController> logger,
            BookRoomService bookRoomService,
            ReservationService reservationService,
            MergeCDService mergeService, CustomerService customerService, OrderService orderService)


        {
            _orderService= orderService;
            _rentRoomService = rentRoomService;
            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _logger = logger;
            _bookRoomService = bookRoomService;
            _mergeService = mergeService;
            _reservationService = reservationService;
            _customerService = customerService;
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index(string? key, DateTime? startDate, bool? temp, int? page, int? size)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now;
            }

            if (!temp.HasValue)
            {
                temp = true;
            }
            if(string.IsNullOrEmpty(key))
            {
                key = "";
            }

            DateTime endDay = startDate.Value.AddDays(6); // Cộng thêm 6 ngày

            var bookRoom = await _bookRoomService.GetAsync(null, page, size, startDate, endDay, temp, null);
            LCount<Room>? room = await _roomService.GetAllAsync(key);
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
            var room = await _roomService.GetAllAsync(null);

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
                    var data = await _bookRoomService.GetAsync("", null, null, startDate, endDate, null,
                        onlyReservedAt);
                    return Json(data);
                }
                catch (HttpRequestException)
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = false });
        }

        public async Task<JsonResult> Quoc(DateTime startDate, DateTime endDate)
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
                var roomItems = await _roomService.GetAllAsync(null);
                var reservation = await _reservationService.GetByIdAsync(id);
                var customer = await _customerService.GetByIdAsync(reservation.CustomerId);


                var viewModel = new MyViewModel
                {
                    Rooms = roomItems.Items,
                    Reservation = reservation,
                    Customer = customer
                };
                return PartialView(viewModel);
            }


            public async Task<IActionResult> RentRoom(string id)
            {
                try
                {
                    var rentRooms = await _rentRoomService.GetAsync(null, null, null);
                    var reservation = await _reservationService.GetByIdAsync(id);
                    var data = new RentBook
                    {
                        reservation = reservation,
                        RentRooms = rentRooms
                    };
                    return PartialView(data);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error retrieving room types");
                    return View("Error");
                }
            }

            [HttpPut]
            public async Task<JsonResult> RentBookRoom([FromBody] RentBookRoom data)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _reservationService.UpdateAsync(data.Reservation);
                        await _reservationDetailService.CreateAsync(data.ReservationDetail);
                    var detail = await _reservationDetailService.GetAsync(data.ReservationDetail.RoomId, false);
                    var order = new Order
                    {
                        ReservationDetailId = detail.Items.FirstOrDefault().Id,
                        Details = new OrderDetail[0]
                    };
                    await _orderService.CreateAsync(order);
                        await _roomService.UpdateAsync(data.Room);
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
            public async Task<JsonResult> PutReservationAndCustumer([FromBody] MergeRC data)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _customerService.CreateAsync(data.Customer);
                        LCount<Customer>? customer =
                            await _customerService.GetAsync(null, null, null, null, null, data.Customer.IdNo);
                        var cus = customer.Items.FirstOrDefault();
                        data.Reservation.CustomerId = cus.Id;
                        await _reservationService.CreateAsync(data.Reservation);
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
            public async Task<JsonResult> PutReservation([FromBody] Reservation data)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _reservationService.CreateAsync(data);
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