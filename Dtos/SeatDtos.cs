using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class SeatDtos{
    public string? SeatName{get;set;}
    public bool IsBooked{get;set;}=false;
}
