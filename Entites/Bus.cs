using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Entites;

public class Bus
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public DeckerType? deckerType { get; set; }

    public string? BusModel { get; set; }
    public int NumberOfExtraSeats { set; get; }
    [Required]
    public int NumberOfRowInCenter { get; set; }
    [Required]
    public int NumberOfSeatsInCenterRight { get; set; }
    [Required]
    public int NumberOfSeatsInCenterleft { get; set; }
    public int NumberOfSeatsInLastSeries { get; set; }
}

public enum DeckerType
{
    SingleDecker = 1,
    DoubleDecker = 2
}