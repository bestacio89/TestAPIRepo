using AutoMapper;

namespace LibCourse.API.Profiles
{
    public class CoursesProfile: Profile  
    {
        public PersonProfileProfile()
        {
            CreateMap<Entities.Person, Models.PersonDto>();
            CreateMap<Models.CourseCreationDto, Entities.Person>();

        }
    }
}
