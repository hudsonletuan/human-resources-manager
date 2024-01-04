using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerWebApplication.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public string? ID { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string? FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string? LastName { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }

        [DisplayName("Local Address")]
        public string? LocalAddress { get; set; }

        [DisplayName("Position ID")]
        public string? PositionID { get; set; }

        [DisplayName("Position")]
        public string? PositionName { get; set; }

        [DisplayName("Department ID")]
        public string? DepartmentID { get; set; }

        [DisplayName("Department")]
        public string? DepartmentName { get; set; }

        [DisplayName("Salary")]
        public string? Salary { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        public Employee()
        {
            UrlImage = ""; // Initialize UrlImage with an empty string
        }
        public string? UrlImage { get; set; }

        [NotMapped]
        public string? FullName
        {
            get { 
                if (MiddleName != null)
                {
                    return FirstName + " " + MiddleName + " " + LastName;
                } else
                {
                    return FirstName + " " + LastName;
                }
            }
        }
    }
}
