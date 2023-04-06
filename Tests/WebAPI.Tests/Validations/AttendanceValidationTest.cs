using FluentValidation;
using FluentValidation.TestHelper;
using Application.ViewModels.AttendanceViewModels;
using Domain.Enums.AttendanceEnums;
using Domain.Tests;
using Infracstructures.Validators;
using WebAPI.Validations.AttendanceValidations;
using Application.IValidators;

namespace WebAPI.Tests.Validations
{
    public class AttendanceValidationTest : SetupTest
    {
        private readonly IAttendanceValidator _validator;
        private readonly IValidator<TakeAttendanceModel> _takeAttendanceValidator;

        public AttendanceValidationTest()
        {
            _takeAttendanceValidator = new TakeAttendanceValidation();
            _validator = new AttendanceValidator(_takeAttendanceValidator);
        }

        [Theory]
        [InlineData(null, AttendanceStatus.Absent, "123")]
        [InlineData(0, AttendanceStatus.Absent, "123")]
  
        public async void TakeAttendanceValidator_WrongInputClassUserID(int ClassUserID, AttendanceStatus? status, string? reason)
        {
            //Assign
            var attendance = new TakeAttendanceModel()
            {
                ClassUserID = ClassUserID,
                AttendanceStatus = status,
                Reason = reason
            };

            //Act
            var result = _validator.TakeAttendanceModel.TestValidate(attendance);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.ClassUserID).Only();
            result.ShouldNotHaveValidationErrorFor(x => x.AttendanceStatus);
            result.ShouldNotHaveValidationErrorFor(x => x.Reason);
        }
        [Theory]
        [InlineData(1, null, "123")]

        public async void TakeAttendanceValidator_WrongInputAttendanceStatus(int ClassUserID, AttendanceStatus? status, string? reason)
        {
            //Assign
            var attendance = new TakeAttendanceModel()
            {
                ClassUserID = ClassUserID,
                AttendanceStatus = status,
                Reason = reason
            };

            //Act
            var result = _validator.TakeAttendanceModel.TestValidate(attendance);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.AttendanceStatus).Only();
            result.ShouldNotHaveValidationErrorFor(x => x.ClassUserID);
            result.ShouldNotHaveValidationErrorFor(x => x.Reason);
        }
        [Theory]
        [InlineData(1, AttendanceStatus.Absent, "123")]
        [InlineData(1, AttendanceStatus.Absent, null)]
        [InlineData(1, AttendanceStatus.Absent, "")]


        public async void TakeAttendanceValidator_AllInputValid(int ClassUserID, AttendanceStatus? status, string? reason)
        {
            //Assign
            var attendance = new TakeAttendanceModel()
            {
                ClassUserID = ClassUserID,
                AttendanceStatus = status,
                Reason = reason
            };

            //Act
            var result = _validator.TakeAttendanceModel.TestValidate(attendance);

            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AttendanceStatus);
            result.ShouldNotHaveValidationErrorFor(x => x.ClassUserID);
            result.ShouldNotHaveValidationErrorFor(x => x.Reason);
        }
    }
}
