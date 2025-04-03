using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class ScheduleBusDtos
{
    public Guid ScheduleId { get; set; }
    [Required]
    public Guid BusId { get; set; }
    
    public string? Name { get; set; }
    [Required]
    public TimeSpan StartingAt { get; set; }
    [Required]
    public DateOnly ScheduledDate { get; set; }
    [Required]
    public string? StartingPlace { set; get; }
    [Required]
    public TheTownBusIsNow? theTownBusIsNow { get; set; }
}
