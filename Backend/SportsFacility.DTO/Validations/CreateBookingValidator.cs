using FluentValidation;
using SportsFacility.DTO;

namespace SportsFacility.API.Validations
{
    // Validations/CreateBookingValidator.cs
    public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.FacilityId)
                .GreaterThan(0)
                .WithMessage("Facility ID must be greater than 0");

            RuleFor(x => x.StartTime)
                .Must(BeFutureDate)
                .WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .Must(BeAfterStartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.EndTime)
                .Must(BeFutureDate)
                .WithMessage("End time must be in the future");

            RuleFor(x => x.MemberId)
                .GreaterThan(0)
                .WithMessage("Member ID must be greater than 0");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters");
        }

        private bool BeFutureDate(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }

        private bool BeAfterStartTime(DateTime endTime)
        {
            return endTime > DateTime.UtcNow;
        }
    }

    // Validations/RegisterMemberValidator.cs
    public class RegisterMemberValidator : AbstractValidator<RegisterMemberDto>
    {
        public RegisterMemberValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(50)
                .WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(50)
                .WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Please provide a valid Indian mobile number");

            RuleFor(x => x.AadhaarNumber)
                .Matches(@"^\d{12}$")
                .WithMessage("Aadhaar number must be 12 digits");
        }
    }

}
