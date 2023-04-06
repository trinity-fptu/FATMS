using Application.ViewModels.AddLectureViewModels;
using FluentValidation;

namespace WebAPI.Validations.LectureValidations
{
    public class AddLectureValidation : AbstractValidator<AddLectureViewModel> 
    {
        public AddLectureValidation()
        {
            //Name
            RuleFor(x => x.NameLecture).NotNull().WithMessage("Lecture name must not be null.")
                .NotEmpty().WithMessage("Lecture name must not be empty.");

            //OutputStandardId
            RuleFor(x => x.OutputStandardId).NotNull().WithMessage("Output Standard Id must not be null.")
                .NotEmpty().WithMessage("Output Standard Id must not be empty.");

            RuleFor(x => x.OutputStandardId)
           .GreaterThan(0)
           .WithMessage("Output Standard Id must greater than 0");
            
            //DeliveryType
            RuleFor(x => x.DeliveryType).NotNull().WithMessage("Delivery type must not be null.")
                .IsInEnum().WithMessage("Delivery type status is invalid.");
        }
    }
}
