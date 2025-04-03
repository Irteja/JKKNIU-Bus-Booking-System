using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Entites;

public class BusSeats{
    [Required]
    public Guid BusId{get;set;}
    [Required]
    public string? SeatName{get;set;}
}