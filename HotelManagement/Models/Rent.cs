﻿using HotelManagementAPI.Models;

namespace HotelManagement.Models
{
    public class Rent
    {
        public List<RentRoom>? RentRooms { get; set; }
        public List<ReOrder>? ReOrders { get; set; }
        public string? Tap { get; set; } = null;

        public List<HistoryRent>? History { get; set; }

        public string Order { get; set; }
    }
}
