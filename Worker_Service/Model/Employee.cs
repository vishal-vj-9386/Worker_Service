using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker_Service.Model
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
    }
}
