using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BOL.Validations
{
    class ValidPasswordAttribute:ValidationAttribute
    {
        override protected ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //password must be with 64 chars, pattern is: [0-9A-Z]
            return ((Regex.Matches(JsonConvert.SerializeObject(value), @"[0-9a-z]").Count)==64) ? null :
                new ValidationResult("Paasword is not valid");
        }
    }
}
