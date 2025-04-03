using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Dtos;

public class StudentLoginDtos
{
    [Required, EmailAddress]
    public string Mail { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}