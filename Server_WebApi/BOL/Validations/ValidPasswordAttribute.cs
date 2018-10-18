using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BOL.Validations
{
    class ValidPasswordAttribute:ValidationAttribute
    {
        override protected ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty("StartDate");
            object propertyValue = property.GetValue(instance);
            DateTime.TryParse(propertyValue.ToString(), out DateTime startDate);
            string passwordSHA = JsonConvert.SerializeObject(value);
            return ((Regex.Matches(passwordSHA, @"[0-9A-Z]").Count)==0) ? null :
                new ValidationResult("Paasword is not valid");
        }
    }
}
