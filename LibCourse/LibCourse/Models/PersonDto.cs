using System;

namespace LibCourse.API.Models
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }    
        public string MainCategory { get; set; }
    }
}
