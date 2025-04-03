using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Entites;

public class ScheduleBus
{
    [Key]
    public Guid ScheduleId { get; set; }
    [Required]
    public Guid BusId { set; get; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public TimeSpan StartingAt { get; set; }
    [Required]
    public DateOnly ScheduledDate { get; set; }
    [Required]
    public string? StartingPlace { set; get; }
    [Required]
    public TheTownBusIsNow? theTownBusIsNow{get;set;}

}

public enum TheTownBusIsNow{
    trishal=1,
    mymenshing=2,
    Valuka=3
}