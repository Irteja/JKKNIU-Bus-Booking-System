using System.ComponentModel.DataAnnotations;
using System;
namespace JKKNIUBusBookingSystem.Entites;

public class Student{
    [Key]
    public Guid id{get;set;}
    [Required]
    public string? Name{get;set;}
    [Required]
    public string? RegistrationNumber{get;set;}
    [Required]
    public string? RollNumber{get;set;}
    [Required]
    public string? Mail{get;set;}
    [Required]
    public string? Phone{get;set;}
    [Required]
    public string? DepartmentName{get;set;}
    [Required]
    public string? Session{set;get;}
    [Required]
    public string? Password{set;get;}
}