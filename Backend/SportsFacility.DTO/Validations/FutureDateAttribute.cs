using System.ComponentModel.DataAnnotations;

namespace SportsFacility.DTO.Validations
{
    // Validations/FutureDateAttribute.cs
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute() : base("Date must be in the future") { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.UtcNow)
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }

            return ValidationResult.Success;
        }
    }
}
