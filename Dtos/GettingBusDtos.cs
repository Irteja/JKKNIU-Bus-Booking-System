using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;
namespace JKKNIUBusBookingSystem.Dtos;

public class GettingBusDtos{

    public Guid Id{get;set;}
    [Required]
    public string? Name{get;set;}
    
}
