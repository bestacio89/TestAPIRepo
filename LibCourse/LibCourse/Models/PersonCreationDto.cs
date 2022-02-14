using System;
using System.Collections.Generic;

namespace LibCourse.API.Models
{
    public class PersonCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseCreationDto> Courses { get; set; } = new List<CourseCreationDto>();
    }
}
