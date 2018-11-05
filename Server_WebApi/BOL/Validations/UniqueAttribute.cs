using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BOL.Help.Validations
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                string fieldName = validationContext.DisplayName.Equals("EMail") ? "email" : "name";

                //Invoke method 'RunObjectReader' from 'DBAccess' in 'DAL project' by reflection (not by adding reference!)

                //1. Load 'DAL' project
                Assembly assembly = Assembly.LoadFrom(@"C:\Users\Libi\Desktop\FinalProject\Server_WebApi\DAL\bin\Debug\DAL.dll");

                //2. Get 'DBAccess' type
                Type DBAccessType = assembly.GetTypes().First(t => t.Name.Equals("DBAccess"));

                //3. Get 'RunReader' method
                MethodInfo RunReaderMethod = DBAccessType.GetMethods().First(m => m.Name.Equals("RunReader"));

                //4. Invoke this method
                string tableName = validationContext.ObjectInstance.GetType().Name.Equals("Login") ? "workers" : "projects";

                string query = $"SELECT * FROM projects_managment.{tableName} WHERE {fieldName} = '{value.ToString()}';";

                Func<MySqlDataReader, bool> func = (reader) =>
                {
                    return (reader.Read());
                };
                MethodInfo generic = RunReaderMethod.MakeGenericMethod(typeof(bool));
                bool isUnique = (bool)generic.Invoke(null, new object[] { query, func });

                if (isUnique == false)
                {
                    ErrorMessage = $"{fieldName} must be unique";
                    validationResult = new ValidationResult(ErrorMessageString);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validationResult;
        }

    }
}