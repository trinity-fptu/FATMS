using Application.ViewModels.SyllabusViewModels;
using FluentValidation;

namespace WebAPI.Validations.SyllabusValidations;

public class BaseSyllabusValidator<T> : AbstractValidator<T> where T : BaseSyllabusViewModel
{
    public BaseSyllabusValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Syllabus Code must not empty");
        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Syllabus Version must greater than 0");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Syllabus Name must not empty");
        RuleFor(x => x.Level)
            .IsInEnum()
            .NotEmpty()
            .WithMessage("Syllabus Level must not empty");
        RuleFor(x => x.AttendeeNumber)
            .GreaterThan(0)
            .WithMessage("Syllabus AttendeeNumber must greater than 0");
        RuleFor(x => x.CourseObjectives)
            .NotEmpty()
            .WithMessage("Syllabus CourseObjectives must not empty");
        RuleFor(x => x.TechnicalRequirements)
            .NotEmpty()
            .WithMessage("Syllabus TechnicalRequirements must not empty");
        RuleFor(x => x.QuizCriteria)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus QuizCriteria must greater than 0 and less than 10");
        RuleFor(x => x.AssignmentCriteria)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus AssignmentCriteria must greater than 0 and less than 10");
        RuleFor(x => x.FinalCriteria)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus FinalCriteria must greater than 0 and less than 10");
        RuleFor(x => x.FinalTheoryCriteria)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus FinalTheoryCriteria must greater than 0 and less than 10");
        RuleFor(x => x.FinalPracticalCriteria)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus FinalPracticalCriteria must greater than 0 and less than 10");
        RuleFor(x => x.PassingGPA)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Syllabus PassingGPA must greater than 0 and less than 10");
        RuleFor(x => x.isActive)
            .NotEmpty()
            .WithMessage("Syllabus isActive must not empty");
    }
}