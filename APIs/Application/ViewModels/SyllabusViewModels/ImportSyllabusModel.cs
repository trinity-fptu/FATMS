using Application.ViewModels.TrainingDeliveryPrincipleViewModels;
using Application.ViewModels.UnitViewModels;
using Domain.Enums.SyllabusEnums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusViewModels
{
    public class ImportSyllabusModel
    {
        public string Code { get; set; }
        public float Version { get; set; }
        public string Name { get; set; }
        public SyllabusLevel Level { get; set; }
        public int AttendeeNumber { get; set; }
        public string CourseObjectives { get; set; }
        public string TechnicalRequirements { get; set; }
        public float QuizCriteria { get; set; }
        public float AssignmentCriteria { get; set; }
        public float FinalCriteria { get; set; }
        public float FinalTheoryCriteria { get; set; }
        public float FinalPracticalCriteria { get; set; }
        public float PassingGPA { get; set; }
        public ImportTrainingDeliveryPrincipleModel TrainingDeliveryPrinciple { get; set; }
        public ICollection<ImportUnitModel> Units { get; set; }
    }
}
