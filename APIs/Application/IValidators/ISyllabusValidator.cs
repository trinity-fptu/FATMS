using Application.ViewModels.SyllabusViewModels;
using FluentValidation;

namespace Application.IValidators;

public interface ISyllabusValidator
{
    IValidator<UpdateSyllabusViewModel> SyllabusAddValidator { get; }
}