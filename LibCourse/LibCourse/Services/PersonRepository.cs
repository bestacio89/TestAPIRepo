using LibCourse.API.DbContexts;
using LibCourse.API.Entities;
using LibCourse.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibCourse.API.Services
{
    public class PersonRepository : ILibCourseRepository, IDisposable
    {
        private readonly LibCourseContext _context;

        public PersonRepository(LibCourseContext context )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid PersonId, Course course)
        {
            if (PersonId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            // always set the PersonId to the passed-in PersonId
            course.PersonId = PersonId;
            _context.Courses.Add(course); 
        }         

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }
  
        public Course GetCourse(Guid PersonId, Guid courseId)
        {
            if (PersonId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.PersonId == PersonId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Course> GetCourses(Guid PersonId)
        {
            if (PersonId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            return _context.Courses
                        .Where(c => c.PersonId == PersonId)
                        .OrderBy(c => c.Title).ToList();
        }

        public void UpdateCourse(Course course)
        {
            // no code in this implementation
        }

        public void AddPerson(Person Person)
        {
            if (Person == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            // the repository fills the id (instead of using identity columns)
            Person.Id = Guid.NewGuid();

            foreach (var course in Person.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Persons.Add(Person);
        }

        public bool PersonExists(Guid PersonId)
        {
            if (PersonId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            return _context.Persons.Any(a => a.Id == PersonId);
        }

        public void DeletePerson(Person Person)
        {
            if (Person == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            _context.Persons.Remove(Person);
        }
        
        public Person GetPerson(Guid PersonId)
        {
            if (PersonId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            return _context.Persons.FirstOrDefault(a => a.Id == PersonId);
        }

        public IEnumerable<Person> GetPersons()
        {
            return _context.Persons.ToList<Person>();
        }
        public IEnumerable<Person> GetPersons(PersonResourceParameters AuthResPam)
        {
            if (AuthResPam == null)
            {
                return GetPersons();
            }
            var collection = _context.Persons as IQueryable<Person>;
           
            if (!string.IsNullOrWhiteSpace(AuthResPam.MainCategory))
            {
              var  mainCategory = AuthResPam.MainCategory.Trim();
                collection = collection.Where(a=>a.MainCategory == mainCategory);

            }

            if (!string.IsNullOrWhiteSpace(AuthResPam.SearchQuery))
            {
                var searchQuery = AuthResPam.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
                ||a.FirstName.Contains(searchQuery)
                ||a.LastName.Contains(searchQuery));


            }
          
            return collection.ToList();

        }
        public IEnumerable<Person> GetPersons(IEnumerable<Guid> PersonIds)
        {
            if (PersonIds == null)
            {
                throw new ArgumentNullException(nameof(PersonIds));
            }

            return _context.Persons.Where(a => PersonIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdatePerson(Person Person)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
