using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BOL.Validations
{
    class MinEndDateAttribute:ValidationAttribute
    {
        override protected ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty("StartDate");
            object propertyValue = property.GetValue(instance);
            DateTime.TryParse(propertyValue.ToString(), out DateTime startDate);

            //end date must be after start date
            return (startDate!=null && (DateTime)value >= startDate) ? null :
                new ValidationResult("End date must be after start date");
        }
    }
}
