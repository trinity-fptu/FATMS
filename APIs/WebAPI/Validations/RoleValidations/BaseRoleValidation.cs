using Application.ViewModels.RoleViewModels;
using Domain.Enums.RoleEnums;
using FluentValidation;

namespace WebAPI.Validations.RoleValidations
{
    public class BaseRoleValidation<T> : AbstractValidator<T> where T : BaseRoleViewModel
    {
        public BaseRoleValidation()
        {
            //Syllabus Permission
            RuleFor(x => x.SyllabusPermission)
                .IsEnumName(typeof(UserPermission)).WithMessage("Syllabus Permission is invalid.");

            //TrainingProgram Permission
            RuleFor(x => x.TrainingProgramPermission)
                .IsEnumName(typeof(UserPermission)).WithMessage("Training Program Permission is invalid.");

            //Class Permission
            RuleFor(x => x.ClassPermission)
                .IsEnumName(typeof(UserPermission)).WithMessage("Class Permission is invalid.");

            //LearningMaterial Permission
            RuleFor(x => x.LearningMaterialPermission)
                .IsEnumName(typeof(UserPermission)).WithMessage("Learning Material Permission is invalid.");

            //User Permission
            RuleFor(x => x.UserPermission)
                .IsEnumName(typeof(UserPermission)).WithMessage("User Permission is invalid.");
        }
    }
}
