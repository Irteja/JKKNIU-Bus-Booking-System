using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Dtos;

public class SeatDetailsForAdminDtos
{
    public string studentName { get; set; } = string.Empty;
    public string registrationNumber { get; set; } = string.Empty;
    public string rollNumber { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string departmentName { get; set; } = string.Empty;
    public string session { set; get; } = string.Empty;
    public string seatName { set; get; } = string.Empty;
}