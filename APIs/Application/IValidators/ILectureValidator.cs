using Application.ViewModels.AddLectureViewModels;
using FluentValidation;

namespace Application.IValidators
{
    public interface ILectureValidator
    {
        IValidator<AddLectureViewModel> AddLectureModel { get; }
    }
}
