using Application.ViewModels.SyllabusViewModels;
using FluentValidation;

namespace WebAPI.Validations.SyllabusValidations;

public class SyllabusUpdateValidation : BaseSyllabusValidator<UpdateSyllabusViewModel>
{
    public SyllabusUpdateValidation()
    {
        
    }
}