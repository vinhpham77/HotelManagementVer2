using HotelManagement.Models;
using HotelManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class MergeCDController
    {

        private readonly RoomService _roomService;
        private readonly ReservationDetailService _reservationDetailService;
        private readonly ReservationService _reservationService;
        private readonly BookRoomService _bookRoomService;
        private readonly MergeCDService _mergeService;


        private readonly ILogger<MergeCDController> _logger;
        public MergeCDController(
            RoomService roomService,
          ReservationDetailService reservationDetailService,
          ILogger<MergeCDController> logger,
          BookRoomService bookRoomService,
          MergeCDService mergeService)
        {

            _roomService = roomService;
            _reservationDetailService = reservationDetailService;
            _logger = logger;
            _bookRoomService = bookRoomService;
            _mergeService= mergeService;
        }
    }
}
