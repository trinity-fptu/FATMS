using Application.IValidators;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Domain.Enums.AttendanceEnums;
using Domain.Enums.LectureEnums;
using FluentValidation;
using FluentValidation.TestHelper;
using Infracstructures.Validators;
using WebAPI.Validations.LectureValidations;

namespace WebAPI.Tests.Validations
{
    public class LectureValidationTest
    {
        private readonly ILectureValidator _validator;
        private readonly IValidator<AddLectureViewModel> _addLectureValidator;

        public LectureValidationTest()
        {
            _addLectureValidator = new AddLectureValidation();
            _validator = new LectureValidator(_addLectureValidator);
        }
        [Theory]
        [InlineData(null, 123, 1, true, LectureDeliveryType.TestQuiz)]
        [InlineData("", 123, 1, true, LectureDeliveryType.TestQuiz)]

        public async void TakeAttendanceValidator_WrongInputNameLecture(string? name, int? duration, int? ouputstandardId
            , bool lessionType, LectureDeliveryType? deliveryType)
        {
            //Assign
            var lecture = new AddLectureViewModel()
            {
                NameLecture = name,
                Duration = duration,
                OutputStandardId = ouputstandardId,
                LessonType = lessionType,
                DeliveryType = deliveryType
            };

            //Act
            var result = _validator.AddLectureModel.TestValidate(lecture);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.NameLecture).Only();
            result.ShouldNotHaveValidationErrorFor(x => x.Duration);
            result.ShouldNotHaveValidationErrorFor(x => x.OutputStandardId);
            result.ShouldNotHaveValidationErrorFor(x => x.LessonType);
            result.ShouldNotHaveValidationErrorFor(x => x.DeliveryType);
        }
        [Theory]
        [InlineData("123", 123, null, true, LectureDeliveryType.TestQuiz)]
        [InlineData("123", 123, 0, true, LectureDeliveryType.TestQuiz)]

        public async void TakeAttendanceValidator_WrongInputOutputStandardID(string? name, int? duration, int? ouputstandardId
           , bool lessionType, LectureDeliveryType? deliveryType)
        {
            //Assign
            var lecture = new AddLectureViewModel()
            {
                NameLecture = name,
                Duration = duration,
                OutputStandardId = ouputstandardId,
                LessonType = lessionType,
                DeliveryType = deliveryType
            };

            //Act
            var result = _validator.AddLectureModel.TestValidate(lecture);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.OutputStandardId).Only();
            result.ShouldNotHaveValidationErrorFor(x => x.Duration);
            result.ShouldNotHaveValidationErrorFor(x => x.NameLecture);
            result.ShouldNotHaveValidationErrorFor(x => x.LessonType);
            result.ShouldNotHaveValidationErrorFor(x => x.DeliveryType);
        }
        [Theory]
        [InlineData("123", 123, 1, true, null)]

        public void TakeAttendanceValidator_WrongInputDeliveryType(string? name, int? duration, int? ouputstandardId
          , bool lessionType, LectureDeliveryType? deliveryType)
        {
            //Assign
            var lecture = new AddLectureViewModel()
            {
                NameLecture = name,
                Duration = duration,
                OutputStandardId = ouputstandardId,
                LessonType = lessionType,
                DeliveryType = deliveryType
            };

            //Act
            var result = _validator.AddLectureModel.TestValidate(lecture);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.DeliveryType).Only();
            result.ShouldNotHaveValidationErrorFor(x => x.Duration);
            result.ShouldNotHaveValidationErrorFor(x => x.NameLecture);
            result.ShouldNotHaveValidationErrorFor(x => x.LessonType);
            result.ShouldNotHaveValidationErrorFor(x => x.NameLecture);
        }
        [Theory]
        [InlineData("123", null, 1, true, LectureDeliveryType.TestQuiz)]
        [InlineData("123", 123, 1, false, LectureDeliveryType.AssignmentLab)]

        public void TakeAttendanceValidator_AllInputValid(string? name, int? duration, int? ouputstandardId
         , bool lessionType, LectureDeliveryType? deliveryType)
        {
            //Assign
            var lecture = new AddLectureViewModel()
            {
                NameLecture = name,
                Duration =duration,
                OutputStandardId = ouputstandardId,
                LessonType = lessionType,
                DeliveryType = deliveryType
            };

            //Act
            var result = _validator.AddLectureModel.TestValidate(lecture);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DeliveryType);
            result.ShouldNotHaveValidationErrorFor(x => x.Duration);
            result.ShouldNotHaveValidationErrorFor(x => x.NameLecture);
            result.ShouldNotHaveValidationErrorFor(x => x.LessonType);
            result.ShouldNotHaveValidationErrorFor(x => x.NameLecture);
        }
    }
}
    
