    using LibCourse.API.Entities;
using LibCourse.API.ResourceParameters;
using System;
using System.Collections.Generic;

namespace LibCourse.API.Services
{
    public interface ILibCourseRepository
    {    
        IEnumerable<Course> GetCourses(Guid PersonId);
        IEnumerable<Person> GetPersons();
        Person GetPerson(Guid PersonId);
        IEnumerable<Person> GetPersons(IEnumerable<Guid> PersonIds);
        public IEnumerable<Person> GetPersons(PersonResourceParameters PersonResPam);
        void AddPerson(Person Person);
        void DeletePerson(Person Person);
        void UpdatePerson(Person Person);
        bool PersonExists(Guid PersonId);
        bool Save();
    }
}
