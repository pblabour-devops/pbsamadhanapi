using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public class CustomeValidationResult
    {
        public bool IsValid { get; set; }
        public ICollection<ValidationResult> CustomeValidationErrorList { get; set; }
        public string ValidationErrorMessages { get; set; }
    }
    public static class CustomeValidator<T>  where T :class
    {
        public static CustomeValidationResult ValidateModel_AllProperties(T obj) 
        {
            CustomeValidationResult customeValidationResult = new CustomeValidationResult();
            ICollection<ValidationResult>  validationResult = new List<ValidationResult>();
            customeValidationResult.IsValid = Validator.TryValidateObject(obj, new ValidationContext(obj), validationResult, true);
            if (!customeValidationResult.IsValid)
            {
                customeValidationResult.ValidationErrorMessages = String.Join("\n", validationResult.Select(o => o.ErrorMessage));
                customeValidationResult.CustomeValidationErrorList = validationResult;
            }
            return customeValidationResult;
        }
    }
}
