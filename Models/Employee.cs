using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(50, ErrorMessage = "Employee name cannot exceed 50 characters.")]
        public string EmployeeName { get; set; }

        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        [Range(0, 1000000000, ErrorMessage = "Salary must be a positive value and .")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Joining is required")]
        public DateTime DateOfJoining { get; set; }
    }
}
