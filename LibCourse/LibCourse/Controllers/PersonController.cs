using LibCourse.API.Services;
using LibCourse.API.Helpers;
using LibCourse.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using LibCourse.API.ResourceParameters;


namespace LibCourse.API.Controllers
{
    [ApiController]
    [Route("api/Persons")]
    public class PersonsController : ControllerBase
    {
        private readonly ILibCourseRepository _LibCourseRepository;
        public readonly IMapper _mapper;
        public PersonsController(ILibCourseRepository LibCourseRepository, IMapper mapper)
        {
            _LibCourseRepository = LibCourseRepository ??
                throw new ArgumentNullException(nameof(LibCourseRepository));
            _mapper = mapper??
                throw new ArgumentNullException(nameof(mapper));    

        }
        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<PersonDto>> GetPersons( [FromQuery] PersonResourceParameters PersonResPar) 
        {
           
            var PersonsFromRepo = _LibCourseRepository.GetPersons(PersonResPar);        
           
            return Ok(_mapper.Map<IEnumerable<PersonDto>>(PersonsFromRepo)); 

        }

        [HttpGet("{PersonId}", Name ="GetPerson")]
        public IActionResult GetPerson(Guid PersonId)
        {
            var PersonFromRepo = _LibCourseRepository.GetPerson(PersonId);
            if (PersonFromRepo==null) 
            {
                return NotFound();  
            }
           
            return Ok(_mapper.Map<PersonDto>(PersonFromRepo));  
        }

        [HttpPost]
        public ActionResult<PersonDto> CreatePerson(PersonCreationDto Person)
        {
            var PersonEntity = _mapper.Map<Entities.Person>(Person);
            _LibCourseRepository.AddPerson(PersonEntity);
            _LibCourseRepository.Save();
            var PersonReturn = _mapper.Map<PersonDto>(PersonEntity);
            return CreatedAtRoute("GetPerson", 
                new { PersonId = PersonReturn.Id}, PersonReturn);
         
        }
        [HttpOptions]
        public IActionResult GetPersonOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
 