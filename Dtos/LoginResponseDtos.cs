using System.ComponentModel.DataAnnotations;
using System;
using JKKNIUBusBookingSystem.Entites;

namespace JKKNIUBusBookingSystem.Dtos;


public class LoginResponseDtos
{
    public string? JwtToken { get; set; }
    public Guid StudentId { get; set; }
}