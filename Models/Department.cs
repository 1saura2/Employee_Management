namespace EmployeeManagement.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Department name cannot exceed 50 characters.")]
        public string DepartmentName { get; set; }
    }
}