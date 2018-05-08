namespace Access2Justice.Repository
{
    using System;    
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ZipCodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as LUISInput;

            if (model == null)
                throw new ArgumentException("Attribute not applied on LUISInput");

            if(value == null)
                return ValidationResult.Success;

            string pattern = @"^\d{5}(\-\d{4})?$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(Convert.ToString(value))) {
                return new ValidationResult("please enter valid zipcode.");
            }
            return ValidationResult.Success;
        }
    }
}
