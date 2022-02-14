using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibCourse.API.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;  
            }

            //Get input value through the value provider
            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).ToString();
            //when said value is Null or whitespace
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;

            }

            //Value isn't a null or whitespace
            //and type of model is Enumerable
            //get the enumerable type, and a converter
            var elementType = bindingContext.ModelType.GetType().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);
            //Convert each item in the value list to the enumerable type
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>converter.ConvertFromString(x.Trim()))
                .ToArray();
           
            //create an array of the type, and set it to model value 
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;
            
            //return a successful result, passing in the model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
