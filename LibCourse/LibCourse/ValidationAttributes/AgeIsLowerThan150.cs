using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibCourse.API.Models;

namespace LibCourse.API.ValidationAttributes
{
    public class AgesISLowerThan150: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var Person = (PersonCreationDto)validationContext.ObjectInstance;
            var now = DateTime.Now;
            var age = new DateTime(DateTime.Now.Subtract(Person.dateofbirth).Ticks).Year - 1;
            if (age >= 150)
            {
                return new ValidationResult("Age doit être inferieure a 150", new[] { "PersonCreationDto" });
            }
            return ValidationResult.Success;
        }

    }
}
