using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Entites;

public class ScheduleBusSeats{
    [Required]
    public Guid ScheduleId{get;set;}
    [Required]
    public string? SeatName{get;set;}
    public Guid? BookedBy{get;set;}
}