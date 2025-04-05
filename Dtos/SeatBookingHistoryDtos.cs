using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class SeatBookingHistoryDtos
{
    public string busName { get; set; } = string.Empty;
    public string seatName { get; set; } = string.Empty;
    public TimeSpan startingAt { get; set; }
    public DateOnly scheduledDate { get; set; }
    public string? startingPlace { set; get; }
    public TheTownBusIsNow? theTownBusWas { get; set; }
}