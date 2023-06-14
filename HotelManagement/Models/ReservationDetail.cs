using Newtonsoft.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelManagement.Models
{
    public class ReservationDetail
    {
        
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("reservationId")]
        public string? ReservationId { get; set; }

        [JsonProperty("roomId")]
        public string? RoomId { get; set; }

        [JsonProperty("checkedInAt")]
        public DateTime CheckedInAt { get; set; }

        [JsonProperty("checkedOutAt")]
        public DateTime? CheckedOutAt { get; set; }

        [JsonProperty("customerId")]
        public string? CustomerId { get; set; }

        [JsonProperty("deposit")]
        public double Deposit { get; set; }

        [JsonProperty("totalAdults")]
        public int TotalAdults { get; set; }

        [JsonProperty("totalChildren")]
        public int TotalChildren { get; set; }

        [JsonProperty("roomPricePerDay")]
        public double RoomPricePerDay { get; set; }

        public int daysIn(DateTime checkedIn, DateTime checkedOut)
        {
            checkedIn.AddHours(12 - checkedIn.Hour);
            checkedOut.AddHours(12 - checkedOut.Hour);
            checkedIn.AddMilliseconds(-checkedIn.Millisecond);
            checkedIn.AddMinutes(-checkedIn.Minute);
            checkedOut.AddMinutes(-checkedOut.Minute);
            TimeSpan timeSpan = checkedOut.Subtract(checkedIn);
            return (int)(timeSpan.TotalDays + 1);
        }

        public double roomPiceDay(ReservationDetail reservationDetail, DateTime checkedIn, DateTime checkedOut)
        {
            return Math.Floor(reservationDetail.RoomPricePerDay * this.daysIn(checkedIn, checkedOut));
        }

        public double roomSurcharge(ReservationDetail reservationDetail, Room room, DateTime checkedIn, DateTime checkedOut)
        {
            double p = reservationDetail.RoomPricePerDay;
            return Math.Floor(p * surchargeCheckIn(checkedIn) + p * surchargeCheckOut(checkedOut) + 200000 * adultsExceed(reservationDetail, room) + 100000 * childrenExceed(reservationDetail, room));
        }

        public double surchargeCheckIn(DateTime checkedIn)
        {
            if (checkedIn.Hour >= 12)
                return 0;
            if (checkedIn.Hour >= 9)
                return 0.3;
            return 0.5;
        }

        public double surchargeCheckOut(DateTime checkedOut)
        {
            if (checkedOut.Hour >= 18)
                return 1;
            if (checkedOut.Hour >= 15)
                return 0.5;
            if (checkedOut.Hour >= 12)
                return 0.3;
            return 0;
        }

        public int adultsExceed(ReservationDetail reservationDetail, Room room)
        {
            if (reservationDetail.TotalAdults > room.MaxAdult)
                return reservationDetail.TotalAdults - room.MaxAdult;
            return 0;
        }

        public int childrenExceed(ReservationDetail reservationDetail, Room room)
        {
            if (reservationDetail.TotalChildren > room.MaxChild)
                return reservationDetail.TotalChildren - room.MaxChild;
            return 0;
        }

        public string hourseSurchargeCheckIn(DateTime checkIn)
        {
            if (checkIn.Hour > 12)
                return "";
            int hour = 12 - checkIn.Hour;
            int minute = 60 - checkIn.Minute;

            if (minute < 60)
                hour--;
            else
                minute = 0;
            return hour + "giờ" + minute + "phút";
        }

        public string hourseSurchargeCheckOut(DateTime checkOut)
        {
            if (checkOut.Hour < 12)
                return "";
            int hour = checkOut.Hour - 12;
            int minute = checkOut.Minute;

            return hour + "giờ" + minute + "phút";
        }
    }
}
