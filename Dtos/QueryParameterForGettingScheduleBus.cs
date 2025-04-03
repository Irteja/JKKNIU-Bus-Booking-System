using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class QueryParameterForGettingScheduleBus
{
    [Required]
    public TimeSpan CurrentTime { get; set; }
    [Required]
    public DateOnly CurrentDate { get; set; }
    
    public string? StartingPlace { set; get; }
}