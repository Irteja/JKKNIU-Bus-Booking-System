using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class RequestBookingDtos{
    [Required]
    public Guid ScheduleId{get;set;}
    [Required]
    public string? SeatName{get;set;}
    public Guid? BookedBy{get;set;}
}
