using Application.IValidators;
using Application.ViewModels.AddLectureViewModels;
using FluentValidation;


namespace Infracstructures.Validators
{
    public class LectureValidator : ILectureValidator
    {
        private readonly IValidator<AddLectureViewModel> _addLectureValidator;

        public LectureValidator(IValidator<AddLectureViewModel> addLectureValidator)
        {
            _addLectureValidator = addLectureValidator;
        }

        public IValidator<AddLectureViewModel> AddLectureModel => _addLectureValidator;
    }
}
