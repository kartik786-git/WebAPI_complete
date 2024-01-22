using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebAPI.Common
{
    public static class ModelValidator
    {

        public static Dictionary<string, object> Validate<T>(T model,
            string[] skipProperties = null!,
            string[] emailProperties = null!)
        {
            if (model == null)
            {
                return new Dictionary<string, object>()

                     { { "Object", "Object does not have a value" } }
                 ;
            }
            // Set to empty arrays if null
            skipProperties ??= Array.Empty<string>();
            emailProperties ??= Array.Empty<string>();
            return ValidateObject(model, "", skipProperties, emailProperties);
        }

        private static Dictionary<string, object> ValidateObject(object obj,
            string currentPath, string[] skipProperties,
            string[] emailProperties)
        {
            Type objType = obj.GetType();
            Dictionary<string, object> errors = new Dictionary<string, object>();
            foreach (PropertyInfo prop in objType.GetProperties())
            {
                string propertyName = $"{currentPath}.{prop.Name}".Trim('.');

                object value = prop.GetValue(obj);

                if (!skipProperties.Contains(propertyName) && 
                    (value == null || 
                    (value is string && string.IsNullOrEmpty((string)value))))
                {
                    //errors.Add(propertyName, $"{propertyName} does not have a value");
                    errors.Add(prop.Name, $"{prop.Name} does not have a value");
                }
                else if (emailProperties != null && emailProperties.Contains(propertyName) 
                    && !IsValidEmail(value.ToString()))
                {
                    //errors.Add(propertyName, $"{propertyName} is not in a valid email format");
                    errors.Add(prop.Name, $"{prop.Name} is not in a valid email format");
                }
                else if (value is IEnumerable enumerable && !(value is string))
                {
                    var subErrors = new List<object>();
                    int index = 0;

                    foreach (var item in enumerable)
                    {
                        var subError = ValidateObject(item, propertyName, skipProperties, emailProperties);
                        if (subError.Count > 0)
                            subErrors.Add(subError);
                        index++;
                    }

                    if (subErrors.Count > 0)
                        //errors.Add(propertyName, subErrors);
                        errors.Add(prop.Name, subErrors);
                }
                else if (value != null && value.GetType().IsClass && value.GetType() != typeof(string))
                {
                    var subErrors = ValidateObject(value, propertyName, skipProperties, emailProperties);
                    if (subErrors.Count > 0)
                        //errors.Add(propertyName, subErrors);
                        errors.Add(prop.Name, subErrors);
                }
                
            }
            return errors;
        }

        private static bool IsValidEmail(string email)
        {
            // Use a simple regex for email validation
            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}