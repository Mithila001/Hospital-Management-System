using HospitalManagementSystem.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Core.Models
{
    public class StaffMember
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        // Consider hashing passwords in a real app, but for a portfolio, plain text might be okay if time is limited,
        // but it's good to mention that in the README. For now, just a string.
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public StaffRole Role { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        // Full Name property for convenience (not mapped to DB by default with EF Core if it has a getter only)
        public string FullName => $"{FirstName} {LastName}";
    }
}