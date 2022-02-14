using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibCourse.API.Entities
{
    [AgeIsLowerThan150]
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }          

      
    }
}
