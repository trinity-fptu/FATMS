using Application.ViewModels.UnitViewModels;
using Domain.Enums.SyllabusEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusViewModels
{
    public class CloneSyllabusViewModel
    {
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool isDeleted { get; set; }
        public string Code { get; set; }
        public float Version { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public SyllabusLevel Level { get; set; }
        public int AttendeeNumber { get; set; }
        public string CourseObjectives { get; set; }
        public string TechnicalRequirements { get; set; }
        public string TrainingDeliveryPrinciple { get; set; }
        public float QuizCriteria { get; set; }
        public float AssignmentCriteria { get; set; }
        public float FinalCriteria { get; set; }
        public float FinalTheoryCriteria { get; set; }
        public float FinalPracticalCriteria { get; set; }
        public float PassingGPA { get; set; }
        public bool isActive { get; set; }
        public int DaysDuration { get; set; }
        public int TimeDuration { get; set; }
        public ICollection<CloneUnitViewModel> Units { get; set; } = new List<CloneUnitViewModel>();
    }
}
