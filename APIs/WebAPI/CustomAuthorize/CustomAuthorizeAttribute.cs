using Microsoft.AspNetCore.Mvc;

namespace WebAPI.CustomAuthorize
{
    public class UserCustomAuthorizeAttribute : TypeFilterAttribute
    {
        public UserCustomAuthorizeAttribute(params string[] permissions) : base(typeof(UserCustomAuthorize))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class SyllabusCustomAuthorizeAttribute : TypeFilterAttribute
    {
        public SyllabusCustomAuthorizeAttribute(params string[] permissions) : base(typeof(SyllabusCustomAuthorize))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class TrainingProgramCustomAutorizeAttribute : TypeFilterAttribute
    {
        public TrainingProgramCustomAutorizeAttribute(params string[] permissions) : base(typeof(TrainingProgramCustomAuthorize))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class ClassCustomAuthorizeAttribute : TypeFilterAttribute
    {
        public ClassCustomAuthorizeAttribute(params string[] permissions) : base(typeof(ClassCustomAuthorize))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class LearningMaterialCustomAuthorizeAttribute : TypeFilterAttribute
    {
        public LearningMaterialCustomAuthorizeAttribute(params string[] permissions) : base(typeof(LearningMaterialCustomAuthorize))
        {
            Arguments = new object[] { permissions };
        }
    }
}